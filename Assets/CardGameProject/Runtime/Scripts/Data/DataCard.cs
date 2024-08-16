using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

namespace CardGameProject
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card Game Project/Create/Card")]
    public class DataCard : DataBase
    {
        [SerializeField]
        private string _cardName = $"{Strings.UNDEFINED}_card";
        [SerializeField]
        private string _cardDescription = Strings.UNDEFINED;
        [SerializeField]
        private Sprite _cardArt = null;
        [SerializeField]
        private int _cardCoast = 0;
        [SerializeField]
        private GameObject _cardPrefab = null;
        [SerializeField]
        private List<DataCardElement> _cardElements = new List<DataCardElement>();

        public string CardName => _cardName;
        public string CardDescription => _cardDescription;
        public Sprite CardArt => _cardArt;
        public int CardCoast => _cardCoast;
        public Card CardPrefab => _cardPrefab?.GetComponent<Card>();
        public IReadOnlyList<DataCardElement> CardElements => _cardElements;

    }
}