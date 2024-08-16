using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMB
{


    [CreateAssetMenu(menuName = "GMB/Data/Data_Card")]
    [ResourcesPath("Cards")]
    public partial class Data_Card : Data, IData_Vendor, IData_Inventory, IData_Elements
    {

        [SerializeField] private Data_CardCategory _category;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _maxStack = 1;
        [SerializeField] private int _gridRows = 1;
        [SerializeField] private int _gridCols = 1;

        [SerializeField] private float _weight = 0.1f;
        [SerializeField] private long _buyPrice = 0;
        [SerializeField] private long _sellPrice = 0;
       
        [SerializeField] private List<Data_Element> _elements = new List<Data_Element>();


        public int GetGridRows()
        {
            return _gridRows;
        }
        public int GetGridCols()
        {
            return _gridCols;
        }
        public IReadOnlyList<Data_Element> GetElements()
        {
            return _elements;
        }
      
        public bool ContainsElementName(string elementName)
        {
            return _elements.Exists(r => r.GetFriendlyName() == elementName);
        }
        public bool ContainsElement(Data_Element element)
        {
            return _elements.Exists(r=>r==element);
        }
        public bool ContainsElementGUID(string guid)
        {
            return _elements.Exists(r => r.GetID() == guid);
        }

        public GameObject GetPrefab()
        {
            return _prefab;
        }
        public T GetPrefab<T>() where T : Component {
            return _prefab.GetComponent<T>();
        }
        public Data_CardCategory GetCategory()
        {
            return _category;
        }
      
        public int GetMaxStack()
        {
            return _maxStack;
        }
        public float GetWeight()
        {
            return _weight;
        }
        public long GetBuyPrice()
        {
            return _buyPrice;
        }
        public long GetSellPrice()
        {
            return _sellPrice;
        }
       
        public override string GetNameAsRelativePath()
        {

            string result = "";

            if (_category != null)
                result += _category.GetFriendlyName();
            else
                result += StringsProvider._UNCATEGORIZED;

            result += "/" + GetFriendlyName();

            return result;
        }


    }
}

