
using GMB;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

namespace CardGameProject
{
    public class CardVisual : MonoBehaviour
    {
        [SerializeField] private Card _card;
        [SerializeField] private CardVisual_Coast _cardCoast;
        [SerializeField] private CardVisual_Element _cardElementPrefab;
        [SerializeField] private RectTransform _elementsContent;
        [SerializeField] private Image _imgCardArt;
        [SerializeField] private TMPro.TextMeshProUGUI _textCardName;
        [SerializeField] private TMPro.TextMeshProUGUI _textCardDescription;

        private List<CardVisual_Element> _elements = new List<CardVisual_Element>();

     
        public void Refresh()
        {
            if (_card == null) { return; }
            
            _imgCardArt.overrideSprite = _card.Data.GetIcon();
            _textCardName.text = _card.Data.GetFriendlyName();
            _textCardDescription.text = _card.Data.GetDescription();
            RefreshCoast();
            RefreshElements();
        }

        public void RefreshCoast()
        {
            if (_card == null) { return; }
            //_cardCoast.Refresh(_card.Data.CardCoast);
            _cardCoast.Refresh(1);
        }

        public void RefreshElements()
        {
            if (_card == null) { return; }
            if (_elements.Count == 0) { RefresPopuledElements(); }
            foreach (CardVisual_Element element in _elements)
            {
                element.Refresh();
            }
        }

        public void RefresPopuledElements()
        {
            ClearElements();
           
            foreach (Data_Element dataElement in _card.Data.GetElements())
            {
                CardVisual_Element instantied = Instantiate(_cardElementPrefab, _elementsContent, false);
                _elements.Add(instantied);
                instantied.ChangeElement(dataElement);
            }
        }

        public void ClearElements()
        {
            foreach (CardVisual_Element element in _elements.ToArray()) //To array to force a copy of the original list. Its prevent array element changed
            {
                Destroy(element.gameObject);
            }
            _elements.Clear();
        }
    }
}