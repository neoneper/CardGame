using GMB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGameProject
{
    /// <summary>
    /// Controlador de Cartas. Este componente cuida do Drag & Drop individual do cartao.
    /// Por meio deste componente voce tem acesso ao <see cref="Deck"/> de cartas caso esta carta pertença a um, bem como ao 
    /// controlador do visual <see cref="CardVisual"/> da carta.
    /// </summary>
    [AddComponentMenu("CardGame/Card")]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CardVisual))]
    public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        private Data_Card _data;
        private CardVisual _cardVisual;
        private CardDeck _currentDeck;
        private IPileNode _currentPileNode;
        private bool _isDragging;
        private int _defaultOrder;

        public Data_Card Data
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
        public CardDeck Deck => _currentDeck;
        public GameObject Node => _currentPileNode?.GetGameObject();
        public IPileNode INode => _currentPileNode;
        public bool IsDragging => _isDragging;

        //INITIASLIZATORS
        public void SetupFromDeck(CardDeck deck, IPileNode pileNode, Data_Card data)
        {
            this._data = data;
            this._currentDeck = deck;
            this._currentPileNode = pileNode;
            Visual.Refresh();
        }
        //METHODS
        public void ClearData()
        {
            _data = null;
        }
        //IDRAG CALLBACKS
        public void OnBeginDrag(PointerEventData eventData)
        {
            _defaultOrder = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            _isDragging = true;
            Deck?.OnBeginCardDrag(this);
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            Deck?.OnCardDrag(this, eventData.position);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            transform.SetSiblingIndex(_defaultOrder);
            Deck?.OnEndCardDrag(this);
        }
        //POINTER CALLBACKS
        public void OnPointerEnter(PointerEventData eventData)
        {
            Deck?.OnCardOverEnter(this);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Deck?.OnCardOverExit(this);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
           
        }
    }
}