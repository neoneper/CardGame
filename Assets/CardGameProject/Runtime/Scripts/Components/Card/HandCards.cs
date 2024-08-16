using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace CardGameProject
{
    public class HandCards : MonoBehaviour
    {
        [SerializeField] private CardDeck _deck;
        [SerializeField] private RectTransform _startPoint;
        [SerializeField] private float _moveSpeed = 10;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _draggingNodeSpaceAmount = 1;
        [SerializeField] private float _normalNodeSpaceAmount = 0.5f;
        [SerializeField] private bool _updateEverTime = false;

        private Dictionary<Card, GameObject> _nodeCardsMap = new Dictionary<Card, GameObject>();
        private Card _cardDragging = null;
        private Card _cardOver = null;
        public CardDeck Deck => _deck;


        private void OnEnable()
        {
            Deck.OnCardAddedEvent += OnCardAdded;
            Deck.OnCardBeginDragEvent += OnCardBeginDrag;
            Deck.OnCardEndDragEvent += OnCardEndDrag;
            Deck.OnCardOverEnterEvent += OnCardOverEnter;
            Deck.OnCardOverExitEvent += OnCardOverExitEvent;
            UpdatePile();
        }
        private void OnDisable()
        {
            Deck.OnCardAddedEvent -= OnCardAdded;
            Deck.OnCardBeginDragEvent -= OnCardBeginDrag;
            Deck.OnCardEndDragEvent -= OnCardEndDrag;
            Deck.OnCardOverEnterEvent -= OnCardOverEnter;
            Deck.OnCardOverExitEvent -= OnCardOverExitEvent;
        }
        private void Update()
        {
            if (_updateEverTime) {UpdatePile(); }

            foreach (Card card in Deck.Cards)
            {

                GameObject node = card.Node;

                if (card.IsDragging)
                {
                    card.transform.rotation = Quaternion.Lerp(card.transform.rotation, Quaternion.identity, _rotationSpeed * Time.deltaTime);
                }
                else
                {
                    card.transform.position = Vector3.Lerp(card.transform.position, node.transform.position, _moveSpeed * Time.deltaTime);
                    card.transform.rotation = Quaternion.Lerp(card.transform.rotation, node.transform.rotation, _rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void UpdatePile()
        {
            _deck.settings.ChangeMaxWidth(_deck.pilePanel.rect.width);
            _deck.settings.ChangeOriginOffset(0, _deck.pilePanel.rect.height / 2);
            _deck.UpdatePile();
        }

        //CALLBACKS
        private void OnCardAdded(Card card)
        {
            card.transform.position = _startPoint.transform.position;
            if (!_updateEverTime)
            {
                UpdatePile();
            }
        }
        private void OnCardOverExitEvent(Card card)
        {
            _cardOver = null;
            card.INode.OffsetPosition = Vector2.zero;
            Deck.spaceNodeAmount = _normalNodeSpaceAmount;
        }

        private void OnCardOverEnter(Card card)
        {
            _cardOver = card;
            card.INode.OffsetPosition = new Vector2(-30,200);
            Deck.spaceNodeAmount = 1;
        }

        private void OnCardEndDrag(Card card)
        {
            _cardDragging = null;
        }

        private void OnCardBeginDrag(Card card)
        {
            _cardDragging = card;
            card.INode.OffsetPosition = Vector2.zero;
            Deck.spaceNodeAmount = 1;
        }


       
    }
}