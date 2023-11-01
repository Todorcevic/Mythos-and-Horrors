using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Debug.Log("OnMuseEnter");
            _currentZoneBahaviour.OnMouseEnter(_cardView);
        }

        public void OnMouseExit()
        {
            Debug.Log("OnMuseExit");
            _currentZoneBahaviour.OnMouseExit(_cardView);
        }

        public void OnMouseDrag()
        {
            _currentZoneBahaviour.OnMouseDrag(_cardView);
        }
    }
}
