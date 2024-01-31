using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class IOActivatorComponent : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private Image _uIBlock;
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _fullBlock;

        public bool IsSensorActivated => !_fullBlock.enabled;
        public bool IsUIActivated => !_uIBlock.enabled;

        /*******************************************************************/
        public void ActivateCardSensors()
        {
            _fullBlock.enabled = false;
        }

        public void DeactivateCardSensors()
        {
            _fullBlock.enabled = true;
        }

        public void UnblockUI()
        {
            _uIBlock.raycastTarget = false;
        }

        public void BlockUI()
        {
            _uIBlock.raycastTarget = true;
        }
    }
}
