using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGameProject
{
    [CreateAssetMenu(fileName = "New Card Catalogue", menuName = "Card Game Project/Create/Card Catalogue")]
    public class DataCardCatalogue : ScriptableObject
    {
        [NonSerialized] private static DataCardCatalogue _instance;
        private static DataCardCatalogue Get
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.LoadAll<DataCardCatalogue>("").ElementAtOrDefault(0);
                    _instance.CheckCatalogue();

                }

                return _instance;
            }
        }

        [SerializeField] List<DataCard> _cards = new List<DataCard>();

        [NonSerialized] Dictionary<string, DataCard> _catalogueByID = new Dictionary<string, DataCard>();

        public static IReadOnlyList<DataCard> Cards => Get._cards;
        public static int CardsCount => Get._cards.Count;

        public static DataCard GetDataCard(string guid)
        {
            DataCardCatalogue instance = Get;
            if (instance._catalogueByID.ContainsKey(guid) == false) { return null; }
            return instance._catalogueByID[guid];
        }
        public static DataCard GetRandom()
        {
            DataCardCatalogue instance = Get;
            int rnd = UnityEngine.Random.Range(0, instance._cards.Count);

            return instance._cards.ElementAt(rnd);

        }

        void CheckCatalogue()
        {
            if (_catalogueByID.Count > 0) { return; }
        }
    }
}