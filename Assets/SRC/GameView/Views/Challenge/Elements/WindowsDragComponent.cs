using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MythosAndHorrors.GameView
{
    public class WindowsDragComponent : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField, Required] private RectTransform _panelBody;
        private Vector3 _pointerDisplacement;

        /*******************************************************************/
        public void OnBeginDrag(PointerEventData eventData) =>
            _pointerDisplacement = Input.mousePosition - _panelBody.position;

        public void OnDrag(PointerEventData eventData) =>
            _panelBody.position = Input.mousePosition - _pointerDisplacement;
    }
}

