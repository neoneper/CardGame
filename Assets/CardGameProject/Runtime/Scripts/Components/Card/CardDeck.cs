using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

namespace CardGameProject
{
    public class CardDeck : PileBehaviour
    {
        public event Action<Card> OnCardAddedEvent;
        public event Action<Card> OnCardRemovedEvent;
        public event Action<Card> OnCardBeginDragEvent;
        public event Action<Card> OnCardEndDragEvent;
        public event Action<Card> OnCardOverEnterEvent;
        public event Action<Card> OnCardOverExitEvent;
        public event Action<Card, Vector2> OnCardDragEvent;

        private ObjectPool<Card> _pool = null;
        private List<DataCard> _requestCards = new List<DataCard>();
        private List<Card> _cards = new List<Card>();

        public IReadOnlyList<Card> Cards => _cards;

        private void Awake()
        {
            _pool = new ObjectPool<Card>(OnCreateCard, OnTakeCardFromPool, OnReturnedCardToPool, OnDestroyPoolCard, true);
        }

        //PUBLIC METHODS
        public void AddRandomCard()
        {
            DataCard data = DataCardCatalogue.GetRandom();
            AddCard(data);
        }
        public void AddCard(DataCard data)
        {

            _requestCards.Add(data);
            AddNode();
        }
        public void RemoveCard(Card card)
        {
            RemoveNode();
        }

        //POLLER CALLBACKS
        private Card OnCreateCard()
        {
            DataCard data = _requestCards.Last();
            _requestCards.Remove(data);
            Card card = Instantiate(data.CardPrefab, transform);
            card.SetupFromDeck(this, GetLastNodeObject(), data);

            return card;
        }
        private void OnTakeCardFromPool(Card card)
        {
            if (card.Data == null)
            {
                DataCard data = _requestCards.Last();
                _requestCards.Remove(data);
                card.SetupFromDeck(this, GetLastNodeObject(), data);
            }
            OnCardAddedEvent.Invoke(card);
            card.gameObject.SetActive(true);

        }
        private void OnReturnedCardToPool(Card card)
        {
            card.ClearData();
            card.gameObject.SetActive(false);

        }
        private void OnDestroyPoolCard(Card card)
        {
            card.ClearData();
            card.gameObject.SetActive(false);
        }
        //PILE CALLBACKS
        protected override void OnNodeAdded(int index)
        {

            _cards.Add(_pool.Get());
        }
        protected override void OnNodeRemoved(int index)
        {


        }
        protected override void OnNodeRemoving(int index)
        {

        }
        //CARDS CALLBACK
        internal void OnBeginCardDrag(Card card)
        {
            OnCardBeginDragEvent?.Invoke(card);
        }
        internal void OnCardDrag(Card card, Vector2 position)
        {
            OnCardDragEvent?.Invoke(card, position);
        }
        internal void OnEndCardDrag(Card card)
        {
            OnCardEndDragEvent?.Invoke(card);
        }
        internal void OnCardOverEnter(Card card)
        {
            OnCardOverEnterEvent?.Invoke(card);
        }
        internal void OnCardOverExit(Card card)
        {
            OnCardOverExitEvent?.Invoke(card);
        }
    }
}