using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PlaceCardView : CardView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _hints;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _enigma;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            if (Card is CardPlace _place)
            {
                _hints.SetStat(_place.Hints);
                _enigma.SetStat(_place.Enigma);
            }
        }

        /*******************************************************************/
        public override void UpdateState() { }
    }
}
