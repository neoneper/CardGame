using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMBEditor
{
    public class GMBEditorListView<T> where T : GMB.Data
    {
        public delegate T ListViewGetInstanceCallBack();
        public delegate List<T> ListViewLoadDataCallback(List<T> loadedItems);
        public delegate void ListViewOnAddButtonCallback(PointerDownEvent evt);
        public delegate void ListViewSelectionCallBack(List<T> items);
        public delegate void ListViewBindCallBack(VisualElement element, T item);
        public delegate ScriptableObject ListViewAddItemCallBack();
        public delegate VisualTreeAsset ListViewMakeItemCallBack();

        public event ListViewSelectionCallBack OnListViewSelectionChangedCallback;
        public event ListViewBindCallBack OnListViewBindItemCallback;
        public event ListViewMakeItemCallBack OnListViewMakeItemCallback;
        public event ListViewGetInstanceCallBack OnListViewGetDataInstanceCallback;
        public event ListViewOnAddButtonCallback OnListViewAddButtonCallback;
        public event ListViewLoadDataCallback OnListViewLoadDatasCallback;

        public List<T> Items { get { return _items.ToList(); } }
        VisualElement _root;
        ListView _listView_items = null;
        Button _buttonAddItem = null;
        Button _buttonRefresh = null;
        Button _buttonRemove = null;
        List<T> _items = null;

        string _path_dataItems = "";

        public void Initialize(VisualElement root, string listView, string addItemButton, string path_dataItems)
        {
            _items = new List<T>();
            this._root = root;
            this._path_dataItems = path_dataItems;


            this._listView_items = root.Q<ListView>(listView);
            this._buttonAddItem = root.Q<Button>(addItemButton);

            _listView_items.selectionChanged += OnSelectionChanged;
            _buttonAddItem.RegisterCallback<PointerDownEvent>(OnButtonAdd, TrickleDown.TrickleDown);

            Refresh();
        }
        public void Initialize(VisualElement root, string listView, string addItemButton, string removeButton, string path_dataItems)
        {
            _items = new List<T>();
            this._root = root;
            this._path_dataItems = path_dataItems;


            this._listView_items = root.Q<ListView>(listView);
            this._buttonAddItem = root.Q<Button>(addItemButton);
            this._buttonRemove = root.Q<Button>(removeButton);

            _listView_items.selectionChanged += OnSelectionChanged;
            _buttonRemove.clicked += OnButtonRemove;
            _buttonAddItem.RegisterCallback<PointerDownEvent>(OnButtonAdd, TrickleDown.TrickleDown);

            Refresh();
        }

        public void Initialize(VisualElement root, string listView, string addItemButton, string removeButton, string refreshButton, string path_dataItems)
        {
            _items = new List<T>();
            this._root = root;
            this._path_dataItems = path_dataItems;


            this._listView_items = root.Q<ListView>(listView);
            this._buttonRefresh = root.Q<Button>(refreshButton);
            this._buttonAddItem = root.Q<Button>(addItemButton);
            this._buttonRemove = root.Q<Button>(removeButton);



            _listView_items.selectionChanged += OnSelectionChanged;
            _buttonAddItem.RegisterCallback<PointerDownEvent>(OnButtonAdd, TrickleDown.TrickleDown);
            _buttonRefresh.clicked += OnButtonRefresh;

            Refresh();
        }
        public void UnInitialize()
        {

            _listView_items.selectionChanged -= OnSelectionChanged;

            if (_buttonAddItem != null)
            {
                _buttonAddItem.UnregisterCallback<PointerDownEvent>(OnButtonAdd, TrickleDown.TrickleDown);
            }

            if (_buttonRefresh != null)
            {
                _buttonRefresh.clicked -= OnButtonRefresh;
            }

            if (_buttonRemove != null)
            {
                _buttonRemove.clicked -= OnButtonRemove;
            }
        }
        public void Refresh()
        {

            if (_items != null)
            {
                _items.Clear();
            }

            List<T> loadedDatas = GMBEditorUtility.FindAllFilesInPath<T>(_path_dataItems);

            if (OnListViewLoadDatasCallback != null)
            {
                _items.AddRange(OnListViewLoadDatasCallback?.Invoke(loadedDatas));
            }
            else
            {
                _items.AddRange(loadedDatas);
            }

            loadedDatas.Clear();
            loadedDatas = null;

            _items = _items.OrderBy(r => r.GetFriendlyName()).ToList();

            _listView_items.itemsSource = _items;

            if (_listView_items.makeItem == null)
            {
                Func<VisualElement> makeItems = () => OnListViewMakeItemCallback?.Invoke().Instantiate();
                Action<VisualElement, int> bindItem = (element, index) =>
                {


                    OnListViewBindItemCallback?.Invoke(element, _items[index]);
                };

                _listView_items.makeItem = makeItems;
                _listView_items.bindItem = bindItem;
            }

            _listView_items.RefreshItems();


        }
        public void RefreshSelectedItem()
        {
            _listView_items.RefreshItem(_listView_items.selectedIndex);
        }
        public void SetSelectionItem(T item)
        {
            if (item == null)
            {
                _listView_items.SetSelection(-1);
                return;
            }
            _listView_items.SetSelection(_listView_items.itemsSource.IndexOf(item));
            RefreshSelectedItem();
        }
        public void SetSelectionItem(int index)
        {
            if (index >= _items.Count)
            {
                if (_listView_items.selectedIndex == -1)
                {
                    OnSelectionChanged(new List<UnityEngine.Object>().AsEnumerable());
                }
                else
                {
                    _listView_items.SetSelection(-1);
                }
                return;
            }
            _listView_items.SetSelection(index);
            RefreshSelectedItem();
        }

        public virtual void OnAddNewIntemFromInstance(T instance)
        {
            if (instance == null)
            {
                Debug.LogError("Erro to create " + instance.name + ".asset, because this have not a IData interface implemantation");
                return;
            }

            instance.SetAutoIncrementID();

            GMBEditorUtility.CreateAsset(instance, _path_dataItems + "/" + instance.GetID() + ".asset");

            Refresh();
            SetSelectionItem((T)instance);
        }
        //CALLBACKS
        private void OnSelectionChanged(IEnumerable<object> obj)
        {

            //T selectedItem = (T)obj.FirstOrDefault();
            OnListViewSelectionChangedCallback?.Invoke(obj.Cast<T>().ToList());

        }
        private void OnButtonRefresh()
        {
            int current_item = _listView_items.selectedIndex;
            Refresh();
            _listView_items.SetSelection(current_item);
        }
        private void OnButtonRemove()
        {

            if (_listView_items.selectedItems.Count() == 0)
                return;

            if (_listView_items.selectedItems.Count() == 1)
            {
                int current_item = _listView_items.selectedIndex;
                current_item -= 1;
                if (GMBEditorUtility.DeleteAsset((GMB.Data)_listView_items.selectedItem))
                {
                    Refresh();
                    _listView_items.SetSelection(current_item);
                }
                else
                {
                    Debug.LogError("Error to delete the asset " + ((GMB.Data)_listView_items.selectedItem).GetFriendlyName());
                }
                return;
            }


            foreach(UnityEngine.Object obj in _listView_items.selectedItems)
            {
                GMB.Data data = obj as GMB.Data;
                GMBEditorUtility.DeleteAsset(data);
            }

            Refresh();


        }
        private void OnButtonAdd(PointerDownEvent evt)
        {

            if (OnListViewAddButtonCallback != null)
            {
                OnListViewAddButtonCallback?.Invoke(evt);
                return;
            }

            T instance = OnListViewGetDataInstanceCallback?.Invoke();
            if (instance == null)
            {
                instance = ScriptableObject.CreateInstance<T>();
            }


            OnAddNewIntemFromInstance(instance);

            //Select the created item in the listview
            // T createdItem = (T)instance;
            // int index = _listView_items.itemsSource.IndexOf(createdItem);
            // _listView_items.SetSelection(index);

        }


    }
}