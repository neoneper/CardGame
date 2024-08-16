using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

namespace CardGameProject
{
    [CreateAssetMenu(fileName = "New Card Element", menuName = "Card Game Project/Create/Card Element")]
    public class DataCardElement : DataBase
    {
        [SerializeField]
        private Sprite _elementIcon = null;

        public string ElementName => DataName;
        public Sprite ElementIcon => _elementIcon;
    }
}