
using System.Collections.Generic;
using UnityEngine;
using GMB;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

namespace GMBEditor
{
    public partial class DataItem_Window : GMBEditorWindow<Data_Item>
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

        GMBEditorTagsView _tags;

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
            _tags.RefreshTagsContent(listview_selectedItem.GetTags());
            _bt_category.text = listview_selectedItem.GetCategory() == null ? "Find" : listview_selectedItem.GetCategory().GetFriendlyName();
        }

        #region PRIVATE UTIL FUNCTIONS
        private void InitializeItem()
        {
            _bt_category = GetElement<Button>("bt_category");
            _bt_element = GetElement<Button>("bt_element");
            _bt_scope = GetElement<Button>("bt_scope");
            _bt_occasion = GetElement<Button>("bt_occasion");
            _bt_usage = GetElement<Button>("bt_usage");

            _tags = new GMBEditorTagsView(this, GetElement<Button>("bt_add_tag"), GetElement<VisualElement>("tags").Q("content"));
            _tags.OnSerializedObjectItemRequest += OnTagObjectItemRequest;

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
            _tags.OnSerializedObjectItemRequest += OnTagObjectItemRequest;
            _tags.Unitialize();
        }
        #endregion

        #region FUNCTION CALLBACKS

        //Tags
        private SerializedObject OnTagObjectItemRequest()
        {
            return listview_selectedItem.GetSerializedObject();
        }
        //Item
        private void OnCategorySearch(PointerDownEvent evt)
        {
            DataEditorUtility.ShowSearchWindow<Data_ItemCategory>("Item Categories", GUIUtility.GUIToScreenPoint(evt.position), OnItem_CategoryRequest);
        }
       
        private void OnItem_CategoryRequest(GMBEditorSearchProvider.SearchResult result)
        {

            if (result.resultFriendlyName == EditorStringsProvider._LISTVIEW_NEW_OPTIONS_)
            {
                GetGMBWindow().OnMenuSelected(typeof(DataItemCategory_Window));
                return;
            }

            SerializedObject serializedObject = listview_selectedItem.GetSerializedObject();
            serializedObject.FindProperty("_category").objectReferenceValue = result.GetDataFile<Data_ItemCategory>();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        private void OnItem_CategoryChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            Data_ItemCategory category = evt.newValue as Data_ItemCategory;
            _bt_category.text = category == null ? "Find" : category.GetFriendlyName();
            listview.RefreshSelectedItem();
        }
        protected override void OnListView_BindItem_Requested(VisualElement element, Data_Item item)
        {
            base.OnListView_BindItem_Requested(element, item);

            Label subTitle = element.Q<Label>("subtitle");
            subTitle.text = item.GetCategory()?.GetFriendlyName();
        }

        #endregion

        #region PROTECTED GETTERS

        protected override string GetTemplate_FilePath()
        {
            return EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Win_Items/Data_Item.uxml";
        }
        protected override string GetTemplate_ListViewItemFilePath()
        {
            return EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_ + "Defaults/Data_Listview_Item_Advanced.uxml";
        }

        public override GMBWindowMenuItem GetGMBWindowMenuItem()
        {
            return new GMBWindowMenuItem(this, "menu_item_items", "Items List", "Items");
        }

        #endregion
    }
}