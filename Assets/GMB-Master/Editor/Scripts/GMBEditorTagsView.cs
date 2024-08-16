using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GMB;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMBEditor
{
    public class GMBEditorTagsView
    {
        public delegate SerializedObject GMBEditorTagsViewSearchCallback();
        public event GMBEditorTagsViewSearchCallback OnSerializedObjectItemRequest;

        VisualElement _tagsContent = null;
        Button _bt_tag = null;
        List<Button> _bufferTagButtons = new List<Button>();
        List<Data_Element> _tags = new List<Data_Element>();
        IGMBEditorWindow _window;
        public string propertyFieldName { get; private set; } = "_tags"; //Array de tags do SerializedObject

        /// <summary>
        /// Cria e Gerencia conteudo de lista <see cref="Data_Element"/>
        /// - Utilize <see cref="OnSerializedObjectItemRequest"/> callback para fornecer o <see cref="Data.GetSerializedObject"/> do item que contem
        /// o array de Tags.
        /// - Por padrao o gerenciador utiliza <see cref="propertyFieldName"/> para localizar o <see cref="SerializedProperty"/>, referente ao array de tags
        /// do objeto.
        ///
        /// - Utilize <see cref="RefreshTagsContent(List{Data_Element})"/> sempre que a lista origem for modificada.
        /// - Utilize <see cref="Unitialize"/> quando o gerenciador nao for mais necessario
        /// - Utilize <see cref="ClearTagsContent"/> para limpar a lista de tags do gerenciador. Isto nao afeta o <see cref="SerializedObject"/> origem
        /// </summary>
        /// <param name="btAddTag">Add Button used to add new tags</param>
        /// <param name="tagsContent">Visual Element content of the tags buttons</param>
        public GMBEditorTagsView(IGMBEditorWindow window, Button btAddTag, VisualElement tagsContent)
        {
            _window = window;
            _tagsContent = tagsContent;
            _bt_tag = btAddTag;
            _bt_tag.RegisterCallback<PointerDownEvent>(OnTagSearch, TrickleDown.TrickleDown);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btAddTag">Add Button used to add new tags</param>
        /// <param name="tagsContent">Visual Element content of the tags buttons</param>
        /// <param name="propertyFieldName">Field name of <see cref="Data_Element"/> List from SerializedObject, requested at <see cref="OnSerializedObjectItemRequest"/></param>
        public GMBEditorTagsView(IGMBEditorWindow window, Button btAddTag, VisualElement tagsContent, string propertyFieldName)
        {
            _window = window;
            this.propertyFieldName = propertyFieldName;
            _tagsContent = tagsContent;
            _bt_tag = btAddTag;
            _bt_tag.RegisterCallback<PointerDownEvent>(OnTagSearch, TrickleDown.TrickleDown);

        }

        public void Unitialize()
        {
            _bt_tag.UnregisterCallback<PointerDownEvent>(OnTagSearch, TrickleDown.TrickleDown);
            OnSerializedObjectItemRequest = null;

        }

        public void ClearTagsContent()
        {
            //Tags 
            foreach (Button bt in _bufferTagButtons)
            {
                bt.clickable.clickedWithEventInfo -= OnTagRemoveRequest;
            }
            _bufferTagButtons.Clear();
            _tagsContent.Clear();
            _tags.Clear();
        }
        public void RefreshTagsContent(List<Data_Element> tags)
        {
            ClearTagsContent();

            _tags.AddRange(tags);
            if (_tags.Count == 0)
                return;


            foreach (Data_Element tag in _tags)
            {
                Button bt = new Button();
                bt.text = tag.GetFriendlyName();
                bt.userData = tag;
                bt.clickable.clickedWithEventInfo += OnTagRemoveRequest;
                _tagsContent.Add(bt);
                _bufferTagButtons.Add(bt);
            }
        }


        private void OnTagSearch(PointerDownEvent evt)
        {
            DataEditorUtility.ShowSearchWindow<Data_Element>("Tags", GUIUtility.GUIToScreenPoint(evt.position), OnItem_TagRequest, _tags);
        }
        private void OnItem_TagRequest(GMBEditorSearchProvider.SearchResult result)
        {
            

            if (result.resultFriendlyName == EditorStringsProvider._LISTVIEW_NONE_OPTIONS_)
            {
                return;
            }

            if (result.resultFriendlyName == EditorStringsProvider._LISTVIEW_NEW_OPTIONS_)
            {
                _window.GetGMBWindow().OnMenuSelected(typeof(DataElements_Window));
                return;
            }


            if (result.GetDataFile<Data_Element>() == null)
            {
                return;
            }

            SerializedObject serializedObject = OnSerializedObjectItemRequest?.Invoke();
            SerializedProperty property = serializedObject.FindProperty(propertyFieldName);
            int index = property.arraySize;
            property.InsertArrayElementAtIndex(index);
            property.GetArrayElementAtIndex(index).objectReferenceValue = result.GetDataFile<Data_Element>();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            List<Data_Element> newList = _tags.ToList();
            newList.Add(result.GetDataFile<Data_Element>());
            RefreshTagsContent(newList);
        }
        private void OnTagRemoveRequest(EventBase obj)
        {
            SerializedObject serializedObject = OnSerializedObjectItemRequest?.Invoke();
            SerializedProperty property = serializedObject.FindProperty(propertyFieldName);
            Data_Element tag = ((Data_Element)((VisualElement)obj.target).userData);

            int index = _tags.IndexOf(tag);

            property.DeleteArrayElementAtIndex(index);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            List<Data_Element> newList = _tags.ToList();
            newList.RemoveAt(index);
            RefreshTagsContent(newList);

        }

    }
}
