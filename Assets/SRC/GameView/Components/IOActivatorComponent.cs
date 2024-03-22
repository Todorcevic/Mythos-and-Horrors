using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class IOActivatorComponent : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private Image _uIBlock;
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _fullBlock;
        [SerializeField, Required, SceneObjectsOnly] private ShowCardInCenterButton _showAllCardsButton;

        /*******************************************************************/
        public void ActivateCardSensors()
        {
            _fullBlock.enabled = false;
        }

        public async Task DeactivateCardSensors()
        {
            if (_fullBlock.enabled) return;
            _fullBlock.enabled = true;
            await DotweenExtension.WaitForAnimationsComplete();
        }

        public Tween UnblockUI()
        {
            _uIBlock.raycastTarget = false;
            return _showAllCardsButton.ActivateToClick();
        }

        public Tween BlockUI()
        {
            _uIBlock.raycastTarget = true;
            return _showAllCardsButton.DeactivateToClick();
        }
    }
}
