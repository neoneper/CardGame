
using UnityEngine;
using CardGameProject.Attributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CardGameProject
{
    public class DataBase : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private string _guid = System.Guid.NewGuid().ToString();
        [SerializeField]
        private string _dataName;
        [SerializeField]
        private string _dataDescription;

        public string Guid => _guid;
        public string DataName => _dataName;
        public string DataDescription => _dataDescription;

        [ContextMenu("RefreshGUID")]
        private void RefreshGUID()
        {
            _guid = System.Guid.NewGuid().ToString();
        }

       public string RawName
        {
            get { return _dataName; }
            set { _dataName = value; }
        }
    }
}
