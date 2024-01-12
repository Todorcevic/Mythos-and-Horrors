using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class IOActivatorComponent : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private Image _blockBackground;
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _boxCollider;

        public bool IsSensorActivated => !_boxCollider.enabled;
        public bool IsUIActivated => !_blockBackground.enabled;
        public bool IsFullyActivated => IsSensorActivated && IsUIActivated;

        /*******************************************************************/
        public void ActivateSensor()
        {
            _boxCollider.enabled = false;
        }

        public void DeactivateSensor()
        {
            _boxCollider.enabled = true;
        }

        public void ActivateUI()
        {
            _blockBackground.enabled = false;
        }

        public void DeactivateUI()
        {
            _blockBackground.enabled = true;
        }
    }
}
