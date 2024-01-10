using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MythsAndHorrors.GameView
{
    public class IOActivatorComponent : MonoBehaviour
    {
        [SerializeField, Required, SceneObjectsOnly] private EventSystem _eventSystem;
        [SerializeField, Required, SceneObjectsOnly] private BoxCollider _boxCollider;

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
            _eventSystem.enabled = true;
        }

        public void DeactivateUI()
        {
            _eventSystem.enabled = false;
        }
    }
}
