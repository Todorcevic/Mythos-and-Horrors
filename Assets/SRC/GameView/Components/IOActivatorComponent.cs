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
        [SerializeField, Required, SceneObjectsOnly] private ShowCardsInCenterButton _showCardsInCenterButton;

        /*******************************************************************/
        public void ActivateCardSensors(bool withShowCenterButton = true)
        {
            if (!_fullBlock.enabled) return;
            _fullBlock.enabled = false;
            _uIBlock.raycastTarget = false;
            if (withShowCenterButton) _showCardsInCenterButton.ActivateToClick();
        }

        public async Task DeactivateCardSensors()
        {
            if (_fullBlock.enabled) return;
            _fullBlock.enabled = true;
            _uIBlock.raycastTarget = true;
            _showCardsInCenterButton.DeactivateToClick();
            await DotweenExtension.WaitForAnimationsComplete();
        }
    }
}
