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
    public class GMBEditorElementsView
    {
        public delegate SerializedObject GMBEditorElementsViewSearchCallback();
        public event GMBEditorElementsViewSearchCallback OnSerializedObjectItemRequest;

        VisualElement _elementsContent = null;
        Button _bt_add_element = null;
        List<Button> _bufferElementsButtons = new List<Button>();
        List<Data_Element> _dataElements = new List<Data_Element>();
        IGMBEditorWindow _window;
        public string propertyFieldName { get; private set; } = "_elements"; //Array de tags do SerializedObject

        /// <summary>
        /// Cria e Gerencia conteudo de lista <see cref="Data_Element"/>
        /// - Utilize <see cref="OnSerializedObjectItemRequest"/> callback para fornecer o <see cref="Data.GetSerializedObject"/> do item que contem
        /// o array de Elements.
        /// - Por padrao o gerenciador utiliza <see cref="propertyFieldName"/> para localizar o <see cref="SerializedProperty"/>, referente ao array de elements
        /// do objeto.
        ///
        /// - Utilize <see cref="RefreshElementsContent(List{Data_Element})"/> sempre que a lista origem for modificada.
        /// - Utilize <see cref="Unitialize"/> quando o gerenciador nao for mais necessario
        /// - Utilize <see cref="ClearElementsContent"/> para limpar a lista de elements do gerenciador. Isto nao afeta o <see cref="SerializedObject"/> origem
        /// </summary>
        /// <param name="btAddElement">Add Button used to add new elements</param>
        /// <param name="elementsContent">Visual Element content of the elements buttons</param>
        public GMBEditorElementsView(IGMBEditorWindow window, Button btAddElement, VisualElement elementsContent)
        {
            _window = window;
            _elementsContent = elementsContent;
            _bt_add_element = btAddElement;
            _bt_add_element.RegisterCallback<PointerDownEvent>(OnElementSearch, TrickleDown.TrickleDown);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btAddElement">Add Button used to add new elements</param>
        /// <param name="elementsContent">Visual Element content of the elements buttons</param>
        /// <param name="propertyFieldName">Field name of <see cref="Data_Element"/> List from SerializedObject, requested at <see cref="OnSerializedObjectItemRequest"/></param>
        public GMBEditorElementsView(IGMBEditorWindow window, Button btAddElement, VisualElement elementsContent, string propertyFieldName)
        {
            _window = window;
            this.propertyFieldName = propertyFieldName;
            _elementsContent = elementsContent;
            _bt_add_element = btAddElement;
            _bt_add_element.RegisterCallback<PointerDownEvent>(OnElementSearch, TrickleDown.TrickleDown);

        }

        public void Unitialize()
        {
            _bt_add_element.UnregisterCallback<PointerDownEvent>(OnElementSearch, TrickleDown.TrickleDown);
            OnSerializedObjectItemRequest = null;

        }

        public void ClearElementsContent()
        {
            foreach (Button bt in _bufferElementsButtons)
            {
                bt.clickable.clickedWithEventInfo -= OnElementRemoveRequest;
            }
            _bufferElementsButtons.Clear();
            _elementsContent.Clear();
            _dataElements.Clear();
        }
        public void RefreshElementsContent(List<Data_Element> elements)
        {
            ClearElementsContent();

            _dataElements.AddRange(elements);
            if (_dataElements.Count == 0)
                return;


            foreach (Data_Element element in _dataElements)
            {
                Button bt = new Button();
                bt.text = element.GetFriendlyName();
                bt.userData = element;
                bt.clickable.clickedWithEventInfo += OnElementRemoveRequest;
                _elementsContent.Add(bt);
                _bufferElementsButtons.Add(bt);
            }
        }


        private void OnElementSearch(PointerDownEvent evt)
        {
            DataEditorUtility.ShowSearchWindow<Data_Element>("Elements", GUIUtility.GUIToScreenPoint(evt.position), OnItem_ElementRequest, _dataElements);
        }
        private void OnItem_ElementRequest(GMBEditorSearchProvider.SearchResult result)
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

            List<Data_Element> newList = _dataElements.ToList();
            newList.Add(result.GetDataFile<Data_Element>());
            RefreshElementsContent(newList);
        }
        private void OnElementRemoveRequest(EventBase obj)
        {
            SerializedObject serializedObject = OnSerializedObjectItemRequest?.Invoke();
            SerializedProperty property = serializedObject.FindProperty(propertyFieldName);
            Data_Element element = ((Data_Element)((VisualElement)obj.target).userData);

            int index = _dataElements.IndexOf(element);

            property.DeleteArrayElementAtIndex(index);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            List<Data_Element> newList = _dataElements.ToList();
            newList.RemoveAt(index);
            RefreshElementsContent(newList);

        }

    }
}
