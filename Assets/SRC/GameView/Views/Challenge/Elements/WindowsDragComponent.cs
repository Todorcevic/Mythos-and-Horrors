using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MythosAndHorrors.GameView
{
    public class WindowsDragComponent : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        private const float Y_TOP_OFFSET = 16f;
        private const float Y_BOTTOM_OFFSET = 64f;

        [SerializeField, Required] private RectTransform _panelBody;
        private Vector3 _pointerDisplacement;

        /*******************************************************************/
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _pointerDisplacement = Input.mousePosition - _panelBody.position;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            _panelBody.position = Input.mousePosition - _pointerDisplacement;
            RepositionateLimit();
        }

        private void RepositionateLimit()
        {
            if (_panelBody.position.x < 0)
                _panelBody.position = new Vector3(0, _panelBody.position.y, 0);
            if (_panelBody.position.x > Screen.width)
                _panelBody.position = new Vector3(Screen.width, _panelBody.position.y, 0);
            if (_panelBody.position.y < Y_BOTTOM_OFFSET)
                _panelBody.position = new Vector3(_panelBody.position.x, Y_BOTTOM_OFFSET, 0);
            if (_panelBody.position.y > Screen.height - Y_TOP_OFFSET)
                _panelBody.position = new Vector3(_panelBody.position.x, Screen.height - Y_TOP_OFFSET, 0);
        }
    }
}

