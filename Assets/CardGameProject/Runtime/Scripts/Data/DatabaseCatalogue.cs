using GMB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGameProject
{
    public class DatabaseCatalogue : MonoBehaviour
    {
        private static DatabaseCatalogue _instance = null;
        public static DatabaseCatalogue Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("DataaseCatalogue");
                    _instance = go.AddComponent<DatabaseCatalogue>();
                }
                return _instance;
            }
        }

        //Precido adicionar uma configuracao global de probjeto runtime para pegar este caminhos.
        private void Awake()
        {
            _cards = Resources.LoadAll<Data_Card>($"{GMB.StringsProvider._RELATIVE_PATH_DATAS_}").ToList();
            DontDestroyOnLoad(gameObject);
        }

        static List<Data_Card> _cards = new List<Data_Card>();
        static Dictionary<string, Data_Card> _catalogueByID = new Dictionary<string, Data_Card>();

        public IReadOnlyList<Data_Card> Cards => _cards;
        public int CardsCount => _cards.Count;

        public Data_Card GetDataCard(string guid)
        {
            if (_catalogueByID.ContainsKey(guid) == false) { return null; }
            return _catalogueByID[guid];
        }
        public Data_Card GetRandomDataCard()
        {
            int rnd = UnityEngine.Random.Range(0, CardsCount);
            return _cards.ElementAt(rnd);
        }


    }
}