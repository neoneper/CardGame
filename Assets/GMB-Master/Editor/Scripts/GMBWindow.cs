using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;
using GMB;

namespace GMBEditor
{

    public class GMBWindowMenuItem
    {
        private string id;
        private string label;
        private string headder;
        private IGMBEditorWindow window;

        public GMBWindowMenuItem(IGMBEditorWindow window, string menuID, string menuLabel, string menuHeadder)
        {
            this.window = window;
            id = menuID;
            label = menuLabel;
            headder = menuHeadder;
        }

        public string MenuID
        {
            get { return id; }
        }
        public string MenuLabel
        {
            get { return label; }
        }
        public string MenuHeadder
        {
            get { return headder; }
        }

        public IGMBEditorWindow Window { get { return window; } }
    }

    public class GMBWindow : EditorWindow
    {
        VisualElement _root; //Todos os demais conteudos estao dentro deste container
        VisualElement _menu_container; //Os menus de janelas e outros conteudos relacionados estao dentro deste container
        VisualElement _menu_content; //A lista de botoes esta dentro deste container
        VisualElement _content_container; //O Conteudo apresentado indivudualmente para cada menu selecionado bem como relacionados esta dentro deste container 
        VisualElement _content; //O Conteudo apresentado indivudualmente para cada menu selecionado esta dentro deste container  
        Button _btnShowMenu;
        //Getters
        public VisualElement menuContent { get { return _menu_content; } }
        public VisualElement menuContainer { get { return _menu_container; } }
        public VisualElement contentContainer { get { return _content_container; } }
        public VisualElement content { get { return _content; } }
        public VisualElement root { get { return _root; } }

        public Color32 selectedMenuColor
        {
            get
            {
                return new Color32(255, 255, 255, 70);
            }
        }
        public Color32 unSelectedMenuColor
        {
            get
            {
                return new Color32(255, 255, 255, 0);
            }
        }

        /// <summary>
        /// <para>string = Headder label, used as group to join menus of the same headder name</para>
        /// <para><see cref="GMBWindowMenuItem"/></para> = the menu settigns
        /// </summary>
        Dictionary<string, List<GMBWindowMenuItem>> menus = new Dictionary<string, List<GMBWindowMenuItem>>();
        List<Button> menuButtons = new List<Button>();


        IGMBEditorWindow currentSelectedWindowMenu;

        [MenuItem("Tools/GMBEditor")]
        public static void ShowWindow()
        {
            GMBWindow wnd = GetWindow<GMBWindow>(false);
            wnd.titleContent = new GUIContent("GMB");
        }

        public void CreateGUI()
        {
            _root = rootVisualElement;
            GetGMBWindowTemplate().CloneTree(root);
            _menu_container = root.Q("root_menu_container");
            _menu_content = menuContainer.Q("content");
            _content_container = _root.Q("root_content_container");
            _content = contentContainer.Q("content");
            _btnShowMenu = root.Q<Button>("Btn_ShowMenu");
            _btnShowMenu.clickable.clicked += OnButtonClicked_ShowMenu;
            BindMenus();

        }
        private void OnDisable()
        {
            _btnShowMenu.clickable.clicked -= OnButtonClicked_ShowMenu;

            if (currentSelectedWindowMenu != null)
            {
                currentSelectedWindowMenu.CloseGUI();
            }

            foreach (Button bt in menuButtons)
            {
                bt.clickable.clickedWithEventInfo -= OnMenuSelected;
            }

            menuButtons.Clear();
            menus.Clear();
            content.Clear();
        }


        public void OnMenuSelected(Type winType)
        {
            Button bt = menuButtons.FirstOrDefault(r => r.userData.GetType() == winType);
            if (bt == null){ return;}

            if (currentSelectedWindowMenu != null)
            {
                RefreshWinButtonHighLiht(GetCurrentMenuWinButton(), false);
                currentSelectedWindowMenu.CloseGUI();
            }

            content.Clear();

            currentSelectedWindowMenu = bt.userData as IGMBEditorWindow;
            currentSelectedWindowMenu.CreateGUI(this);
            RefreshWinButtonHighLiht(bt,true);
        }
        private void OnMenuSelected(EventBase obj)
        {
           
            if (currentSelectedWindowMenu != null)
            {
                RefreshWinButtonHighLiht(GetCurrentMenuWinButton(), false);
                currentSelectedWindowMenu.CloseGUI();
            }

            content.Clear();

            VisualElement target = obj.target as VisualElement;
            currentSelectedWindowMenu = (IGMBEditorWindow)target.userData;
            currentSelectedWindowMenu.CreateGUI(this);

            RefreshWinButtonHighLiht(GetCurrentMenuWinButton(), true);
        }
        private void OnButtonClicked_ShowMenu()
        {
            if (menuContainer.style.display == DisplayStyle.None)
            {
                menuContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                menuContainer.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Procura e cria dinamicamente instancias para todos as implementacoes de <see cref="GMBEditorWindow{T}"/>, adicionando automaticamente
        /// a caada referencia de botao de menu.
        /// <para>Tenha em mente que o botao sera assosiado a instancia da janela atraves de <see cref="IGMBEditorWindow.GetGMBWindowMenuItem"/></para>,
        /// assim sendo retorne extamente o nome referenciado em cada <see cref="Button"/> em suas implementacoes de janela
        /// </summary>
        private void BindMenus()
        {
            var type = typeof(IGMBEditorWindow);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            foreach (Type i in types)
            {
                //Pupule Dictionary of menus group
                IGMBEditorWindow win = (IGMBEditorWindow)Activator.CreateInstance(i);

                GMBWindowMenuItem menu = win.GetGMBWindowMenuItem();


                if (menus.ContainsKey(menu.MenuHeadder))
                {

                    menus[menu.MenuHeadder].Add(menu);
                }
                else
                {

                    menus.Add(menu.MenuHeadder, new List<GMBWindowMenuItem>());
                    menus[menu.MenuHeadder].Add(menu);
                }

            }

            //Ordering menus group
            menus = menus.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

            //Creating visual elements
            foreach (string headder in menus.Keys)
            {

                int headderElementIndex;
                int headderElementsCount;
                GetGMBWindowMenuItemHeadderTemplate().CloneTree(_menu_content, out headderElementIndex, out headderElementsCount);

                VisualElement headderElement = _menu_content.ElementAt(headderElementIndex);
                headderElement.name = headder;
                headderElement.Q<Label>().text = headder.ToUpper();

                foreach (GMBWindowMenuItem menuItem in menus[headder])
                {
                    int buttonElementIndex;
                    int buttonElementsCount;

                    GetGMBWindowMenuItemTemplate().CloneTree(_menu_content, out buttonElementIndex, out buttonElementsCount);

                    VisualElement buttonItemElement = _menu_content.ElementAt(buttonElementIndex);
                    Button but = buttonItemElement.Q<Button>();
                    buttonItemElement.Q<Label>("menu_item_label").text = menuItem.MenuLabel;
                    but.userData = menuItem.Window;
                    but.clickable.clickedWithEventInfo += OnMenuSelected;

                    menuButtons.Add(but);
                }

            }

        }

        private VisualTreeAsset GetGMBWindowTemplate()
        {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Win_Root/Content.uxml");

        }
        private VisualTreeAsset GetGMBWindowMenuItemHeadderTemplate()
        {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Win_Root/Menu_Headder.uxml");

        }
        private VisualTreeAsset GetGMBWindowMenuItemTemplate()
        {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Win_Root/Menu_Item.uxml");

        }
        private Button GetCurrentMenuWinButton()
        {
            return menuButtons.FirstOrDefault(r => r.userData.GetType() == currentSelectedWindowMenu.GetType());
        }
        private void RefreshWinButtonHighLiht(Button button, bool selected)
        {
            StyleColor styleColor = new StyleColor(selected ? selectedMenuColor : unSelectedMenuColor);
            button.parent.style.backgroundColor = styleColor;
        }

    }
}