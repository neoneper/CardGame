using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMB
{


    [CreateAssetMenu(menuName = "GMB/Data/Data_Card")]
    [ResourcesPath("Cards")]
    public partial class Data_Card : Data, IData_Vendor, IData_Inventory, IData_Tags
    {

        [SerializeField] private Data_CardCategory _category;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _maxStack = 1;
        [SerializeField] private int _gridRows = 1;
        [SerializeField] private int _gridCols = 1;

        [SerializeField] private float _weight = 0.1f;
        [SerializeField] private long _buyPrice = 0;
        [SerializeField] private long _sellPrice = 0;
        [SerializeField] private int _tagsValue = 0;
       
        [SerializeField] private List<Data_Element> _tags = new List<Data_Element>();


        public int GetGridRows()
        {
            return _gridRows;
        }
        public int GetGridCols()
        {
            return _gridCols;
        }
        public List<Data_Element> GetTags()
        {
            return _tags;
        }
        public List<string> GetTagsName()
        {
            return _tags.Select(r => r.GetFriendlyName()).ToList();
        }
        public bool ContainsTagName(string tag)
        {
            return _tags.Count(r => r.GetFriendlyName() == tag) > 0;
        }
        public int GetTagIndex(Data_Element tag)
        {
            return _tags.IndexOf(tag);
        }

        public GameObject GetPrefab()
        {
            return _prefab;
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

