using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class IOActivatorComponent : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private Image _uIBlock;
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _fullBlock;

        /*******************************************************************/
        public void ActivateCardSensors()
        {
            _fullBlock.enabled = false;
        }

        public async Task DeactivateCardSensors()
        {
            if (_fullBlock.enabled) return;
            _fullBlock.enabled = true;
            await DotweenExtension.WaitForAllTweensToComplete();
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
