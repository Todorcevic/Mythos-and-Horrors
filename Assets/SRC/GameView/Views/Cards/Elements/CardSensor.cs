using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardSensor : MonoBehaviour
    {
        [SerializeField, Required] private CardView _cardView;
        private IZoneBehaviour _currentZoneBahaviour;

        /*******************************************************************/
        public void SetZoneBahaviour(IZoneBehaviour zoneBahaviour)
        {
            _currentZoneBahaviour = zoneBahaviour;
        }

        /*******************************************************************/
        public void OnMouseEnter()
        {
            DOTween.KillAll();
            _currentZoneBahaviour.MouseEnter(_cardView);
        }

        public void OnMouseExit()
        {
            _currentZoneBahaviour.MouseExit(_cardView);
        }

        public void OnMouseDrag()
        {
            _currentZoneBahaviour.MouseDrag(_cardView);
        }
    }
}
