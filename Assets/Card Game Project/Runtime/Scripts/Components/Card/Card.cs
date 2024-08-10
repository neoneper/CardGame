using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameProject
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CardVisual))]
    public class Card : MonoBehaviour
    {
        [SerializeField] DataCard _data;

        private CardVisual _cardVisual;

        public DataCard Data
        {
            get
            {
                return _data;
            }
        }

        public CardVisual Visual
        {
            get
            {
                if (_cardVisual == null) { _cardVisual = GetComponent<CardVisual>(); }
                return _cardVisual;
            }
        }

        private void OnEnable()
        {
            Visual.Refresh();
        }
    }
}