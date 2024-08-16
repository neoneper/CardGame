
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardGameProject
{
    public class CardVisual_Coast : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _textCoast;

        public int CurrentValue
        {
            get
            {
                if (_textCoast == null) { return 0; }
                return int.Parse(_textCoast.text);
            }
            private set
            {
                _textCoast.text = value.ToString();
            }
        }

        public void Refresh(int coastValue)
        {
            if (_textCoast == null)
            {
                Debug.LogError($"{gameObject.name}: Cant Refresh Coast Value because the TextMeshUGUI is null");
                return;
            }

            CurrentValue = coastValue;
        }
    }
}