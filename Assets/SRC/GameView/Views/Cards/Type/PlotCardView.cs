using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PlotCardView : CardView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _eldritch;

        /*******************************************************************/
        protected override void SetAll()
        {
            _eldritch.text = Card.Info.Eldritch.ToString();
        }
    }
}
