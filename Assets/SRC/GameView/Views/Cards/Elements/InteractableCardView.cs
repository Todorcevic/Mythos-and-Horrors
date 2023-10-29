using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class InteractableCardView : MonoBehaviour
    {
        [SerializeField, Required] private CardView _cardView;
        private IZoneBahaviour _currentZoneBahaviour;

        /*******************************************************************/
        public void SetZoneBahaviour(IZoneBahaviour zoneBahaviour)
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

        public void OnMouseUpAsButton()
        {
            _currentZoneBahaviour.OnClicked(_cardView);
        }
    }
}
