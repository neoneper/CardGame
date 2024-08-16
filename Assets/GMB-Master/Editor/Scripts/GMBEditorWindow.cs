using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections;
using UnityEngine;
using GMB;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;

namespace GMBEditor
{
    public abstract class GMBEditorWindow<T> : IGMBEditorWindow where T : Data
    {



        private GMBWindow _gmbWindow = null;
        private GMBEditorListView<T> _listview = null;
        private T _listview_selectedItem = null;
        private VisualElement _content;
        private VisualElement _noContent;
        private VisualElement _content_thumb;
        private TextField _content_textField_DataName;
        private ObjectField _content_objectField_DataIcon;
        private Button _content_button_remove;

        protected VisualElement gmbContentVisualElement { get { return _gmbWindow.content; } }
        protected VisualElement rootVisualElement { get { return gmbContentVisualElement; } }
        protected VisualElement contentVisualElement { get { return _content; } }
        protected GMBEditorListView<T> listview { get { return _listview; } }

        protected T listview_selectedItem { get { return _listview_selectedItem; } }


        public GMBEditorWindow()
        {
            _listview = new GMBEditorListView<T>();
        }

        public GMBWindow GetGMBWindow()
        {
            return _gmbWindow;
        }

        public void CreateGUI(GMBWindow gmbWindow)
        {
            _gmbWindow = gmbWindow;
            LoadContent();
            CreateListViewItems();

            OnCreateGUI();
        }
        public void CloseGUI()
        {
            UnBind_Content();
            Unregister_Content();
            Unregister_ListView();
            OnCloseGUI();
        }

        //Privates
        private void LoadContent()
        {
            //Load And Clone Content Template
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetTemplate_FilePath()).CloneTree(rootVisualElement);

            //Asign VisualElemets Refs
            _content = rootVisualElement.Q("content_container");
            _noContent = rootVisualElement.Q("nocontent");
            _content_thumb = _content.Q("content_thumb");
            _content_textField_DataName = _content.Q<TextField>("data_name");
            _content_objectField_DataIcon = _content.Q<ObjectField>("data_icon");
            _content_button_remove = _content.Q<Button>("bt_remove");

            _content.SetDisplay(false);
            _noContent.SetDisplay(true);

            //Register to Input Changes Events
            _content_textField_DataName.RegisterValueChangedCallback(OnContent_DataName_Changed);
            _content_objectField_DataIcon.RegisterValueChangedCallback(OnContent_DataIcon_Changed);
            _content_button_remove.clicked += OnContent_DeleteItem_Requested;
        }
        private void CreateListViewItems()
        {
            _listview.Initialize(rootVisualElement, "listview", "bt_add", DataEditorUtility.GetAbsoluteResourcesDatasPath<T>());
            _listview.OnListViewMakeItemCallback += OnListView_MakeItem_Requested;
            _listview.OnListViewBindItemCallback += OnListView_BindItem_Requested;
            _listview.OnListViewSelectionChangedCallback += OnListView_SelectedItem_Changed;
        }
        private void UnBind_Content()
        {
            //Content Unbindings
            _content.Unbind();
        }
        private void Unregister_Content()
        {


            //Content UnRegister from Input Changes Events
            _content_textField_DataName.UnregisterValueChangedCallback(OnContent_DataName_Changed);
            _content_objectField_DataIcon.UnregisterValueChangedCallback(OnContent_DataIcon_Changed);
            _content_button_remove.clicked -= OnContent_DeleteItem_Requested;
        }
        private void Unregister_ListView()
        {
            //ListView Unregisters
            _listview.UnInitialize();
            _listview.OnListViewMakeItemCallback -= OnListView_MakeItem_Requested;
            _listview.OnListViewBindItemCallback -= OnListView_BindItem_Requested;
            _listview.OnListViewSelectionChangedCallback -= OnListView_SelectedItem_Changed;
        }

        //ListView Requested Callbacks
        protected VisualTreeAsset OnListView_MakeItem_Requested()
        {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetTemplate_ListViewItemFilePath());
        }
        protected virtual void OnListView_BindItem_Requested(VisualElement element, T item)
        {

            Label title = element.Q<Label>("title");
            Label subTitle = element.Q<Label>("subtitle");
            VisualElement thumb = element.Q<VisualElement>("img");
            title.text = item.GetFriendlyName();

            Sprite icon = item.GetIcon();
            if (icon == null)
            {
                icon = GMBEditorStyles.SpriteUnKnow;
            }

            thumb.style.backgroundImage = new StyleBackground(icon);
            // subTitle.text = item.GetCategory() == null ? StringsProvider._NONE_ + " - Category" : item.GetCategory().GetName();
            subTitle.text = StringsProvider._UNDEFINED_;

        }

        //ListView Changed Callbacks
        private void OnListView_SelectedItem_Changed(List<T> items)
        {
            UnBind_Content();

            if (items.Count == 0)
            {
                _listview_selectedItem = default(T);
                _content.SetDisplay(false);
                _noContent.SetDisplay(true);

            }
            else
            {
                T item = items.First();

                _content.Bind(item.GetSerializedObject());
                _listview_selectedItem = item;
                _content.SetDisplay(true);
                _noContent.SetDisplay(false);
                OnSelectedItemChanged();
            }

        }

        //Content Requested Callbacks
        private void OnContent_DeleteItem_Requested()
        {
            GMBEditorUtility.DeleteAsset(_listview_selectedItem);
            listview.Refresh();
            listview.SetSelectionItem(null);
        }
        //Content Changed Callbacks
        private void OnContent_DataIcon_Changed(ChangeEvent<UnityEngine.Object> evt)
        {

            Sprite sprite = evt.newValue as Sprite;
            _content_thumb.style.backgroundImage = sprite != null ? new StyleBackground(sprite) : GMBEditorStyles.TextureUnKnow;


            if (listview_selectedItem == null)
            { return; }

            listview.RefreshSelectedItem();
        }
        private void OnContent_DataName_Changed(ChangeEvent<string> evt)
        {
            if (listview_selectedItem == null)
            { return; }

            listview.RefreshSelectedItem();
        }

        //ABSTRACT Getters
        protected abstract string GetTemplate_FilePath();
        protected abstract string GetTemplate_ListViewItemFilePath();
        public abstract GMBWindowMenuItem GetGMBWindowMenuItem();

        //ABSTRACT And Virtuais Events Trigged
        protected abstract void OnCreateGUI();
        protected abstract void OnCloseGUI();
        protected virtual void OnSelectedItemChanged() { }

        //Utils Funtions
        protected E GetElement<E>(string elementName) where E : VisualElement
        {
            return _content.Q<E>(elementName);
        }


    }
}