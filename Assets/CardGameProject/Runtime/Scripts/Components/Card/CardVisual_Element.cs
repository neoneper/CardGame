
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CardGameProject
{
    public class CardVisual_Element : MonoBehaviour
    {
        [SerializeField] private Image _imgElement;

        private DataCardElement _cardElement;
        public DataCardElement CurrentElement => _cardElement;

        /// <summary>
        /// Just refresh the Image Icon using the <see cref="CurrentElement"/> Data.
        /// Take a look in <see cref="ChangeElement(DataCardElement)"/> to change my current element and automaticaly refresh the UI Image.
        /// </summary>
        public void Refresh()
        {
            if (_cardElement == null) { return; }
            this._imgElement.overrideSprite = this._cardElement.ElementIcon;
        }

        /// <summary>
        /// Change my <see cref="CurrentElement"/> but dont Refresh the UI Image icon.
        /// If you want refresh the UI Image, take a look at: <seealso cref="Refresh"/>
        /// </summary>
        /// <param name="element"></param>
        public void ChangeElement(DataCardElement element)
        {
            if (this._imgElement == null)
            {
                Debug.LogError($"{gameObject.name}: Cant Refresh {nameof(this._imgElement)} sprite Value because this UI.Image property is null");
                return;
            }

            if (this._imgElement == null)
            {
                Debug.LogError($"{gameObject.name}: Cant Refresh {nameof(this._imgElement)} sprite Value because this element asset parameter is null");
                return;
            }

            this._cardElement = element;

        }
    }
}