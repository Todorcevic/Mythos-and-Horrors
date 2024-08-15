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
        [SerializeField, Required, SceneObjectsOnly] private ShowCardsInCenterButton _showCardsInCenterButton;

        /*******************************************************************/
        public void ActivateCardSensors()
        {
            if (!_fullBlock.enabled) return;
            _fullBlock.enabled = false;
            UnblockUI();
        }

        public async Task DeactivateCardSensors()
        {
            if (_fullBlock.enabled) return;
            _fullBlock.enabled = true;
            BlockUI();
            await DotweenExtension.WaitForAnimationsComplete();
        }

        //public void DeactivateCardSensorsWithotWait()
        //{
        //    if (_fullBlock.enabled) return;
        //    _fullBlock.enabled = true;
        //    BlockUI();
        //}

        private Tween UnblockUI()
        {
            _uIBlock.raycastTarget = false;
            return _showCardsInCenterButton.ActivateToClick();
        }

        private Tween BlockUI()
        {
            _uIBlock.raycastTarget = true;
            return _showCardsInCenterButton.DeactivateToClick();
        }
    }
}
