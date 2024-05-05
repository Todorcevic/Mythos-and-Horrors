using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class RegisterCardView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _cardImage;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _element;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _container;

        /*******************************************************************/
        public async void Set(string cardCode, int amountXP)
        {
            await _cardImage.LoadCardSprite(cardCode);

            for (int i = 0; i < amountXP; i++)
            {
                Instantiate(_element, _container).gameObject.SetActive(true);
            }
        }
    }
}
