
using System.Collections.Generic;
using UnityEngine;
using GMB;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

namespace GMBEditor
{
    public partial class DataCard_Window : GMBEditorWindow<Data_Card>
    {

        Button _bt_category;
        Button _bt_element;
        Button _bt_scope;
        Button _bt_usage;
        Button _bt_occasion;

        ObjectField _objectField_category;
        ObjectField _objectField_usage;
        ObjectField _objectField_element;
        ObjectField _objectField_scope;
        ObjectField _objectField_occasion;

        GMBEditorElementsView _elements;

        //WIndow Callbacks
        protected override void OnCreateGUI()
        {

            InitializeItem();
           
        }
        protected override void OnCloseGUI()
        {
            UnInitializeItem();
          

        }
        protected override void OnSelectedItemChanged()
        {
            _elements.RefreshElementsContent(listview_selectedItem.GetElements().ToList());
            _bt_category.text = listview_selectedItem.GetCategory() == null ? "Find" : listview_selectedItem.GetCategory().GetFriendlyName();

            if (listview_selectedItem != null)
            {
                GetGMBWindow().AddHistoric(this, listview_selectedItem.GetFriendlyName());
            }
        }

        #region PRIVATE UTIL FUNCTIONS
        private void InitializeItem()
        {
           
            _bt_category = GetElement<Button>("bt_category");
            _bt_element = GetElement<Button>("bt_element");
            _bt_scope = GetElement<Button>("bt_scope");
            _bt_occasion = GetElement<Button>("bt_occasion");
            _bt_usage = GetElement<Button>("bt_usage");

            _elements = new GMBEditorElementsView(this, GetElement<Button>("bt_add_tag"), GetElement<VisualElement>("elements").Q("content"));
            _elements.OnSerializedObjectItemRequest += OnElementDataFieldRequest;

            _objectField_category = GetElement<ObjectField>("data_category");
            _objectField_element = GetElement<ObjectField>("data_element");
            _objectField_occasion = GetElement<ObjectField>("data_occasion");
            _objectField_scope = GetElement<ObjectField>("data_scope");
            _objectField_usage = GetElement<ObjectField>("data_usage");

            _bt_category.RegisterCallback<PointerDownEvent>(OnCategorySearch, TrickleDown.TrickleDown);
            _objectField_category.RegisterValueChangedCallback(OnItem_CategoryChanged);

        }
        private void UnInitializeItem()
        {
            _bt_category.UnregisterCallback<PointerDownEvent>(OnCategorySearch, TrickleDown.TrickleDown);
            _objectField_category.UnregisterValueChangedCallback(OnItem_CategoryChanged);
            _elements.OnSerializedObjectItemRequest += OnElementDataFieldRequest;
            _elements.Unitialize();
        }
        #endregion

        #region FUNCTION CALLBACKS

        //Elements
        private SerializedObject OnElementDataFieldRequest()
        {
            return listview_selectedItem.GetSerializedObject();
        }
        //Category
        private void OnCategorySearch(PointerDownEvent evt)
        {
            DataEditorUtility.ShowSearchWindow<Data_CardCategory>("Card Categories", GUIUtility.GUIToScreenPoint(evt.position), OnItem_CategoryRequest);
        }
       
        private void OnItem_CategoryRequest(GMBEditorSearchProvider.SearchResult result)
        {

            if (result.resultFriendlyName == EditorStringsProvider._LISTVIEW_NEW_OPTIONS_)
            {
                GetGMBWindow().OnMenuSelected(typeof(DataCardCategory_Window));
                return;
            }

            SerializedObject serializedObject = listview_selectedItem.GetSerializedObject();
            serializedObject.FindProperty("_category").objectReferenceValue = result.GetDataFile<Data_CardCategory>();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        private void OnItem_CategoryChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            Data_CardCategory category = evt.newValue as Data_CardCategory;
            _bt_category.text = category == null ? "Find" : category.GetFriendlyName();
            listview.RefreshSelectedItem();
        }
        protected override void OnListView_BindItem_Requested(VisualElement element, Data_Card item)
        {
            base.OnListView_BindItem_Requested(element, item);

            Label subTitle = element.Q<Label>("subtitle");
            subTitle.text = item.GetCategory()?.GetFriendlyName();
        }
        
        #endregion

        #region PROTECTED GETTERS

        protected override string GetTemplate_FilePath()
        {
            return EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Win_Cards/Data_Card.uxml";
        }
        protected override string GetTemplate_ListViewItemFilePath()
        {
            return EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Defaults/Data_Listview_Item_Advanced.uxml";
        }

        public override GMBWindowMenuItem GetGMBWindowMenuItem()
        {
            return new GMBWindowMenuItem(this, "menu_item_cards", "Cards", "Cards");
        }

        #endregion
    }
}