
using UnityEngine;
using CardGameProject.Attributes;

namespace CardGameProject
{
    public class DataBase : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private string _guid = System.Guid.NewGuid().ToString();

        [ContextMenu("RefreshGUID")]
        private void RefreshGUID()
        {
            _guid = System.Guid.NewGuid().ToString();
        }
    }
}
