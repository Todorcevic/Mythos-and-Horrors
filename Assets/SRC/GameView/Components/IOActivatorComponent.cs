using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class IOActivatorComponent : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private Image _blockBackground;
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _boxCollider;

        public bool IsSensorActivated => !_boxCollider.enabled;
        public bool IsUIActivated => !_blockBackground.enabled;

        /*******************************************************************/
        public void ActivateSensor()
        {
            _boxCollider.enabled = false;
        }

        public async Task DeactivateSensor()
        {
            _boxCollider.enabled = true;
            await DotweenExtension.WaitForAllTweensToComplete();
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
