using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PlaceCardView : CardView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _hints;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _enigma;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _hints.text = Card.Info.Hints.ToString();
            _enigma.text = Card.Info.Enigma.ToString();
        }
    }
}
