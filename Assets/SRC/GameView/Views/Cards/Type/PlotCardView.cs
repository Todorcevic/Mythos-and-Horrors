using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PlotCardView : CardView
    {
        [Title(nameof(PlotCardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _eldritch;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            if (Card is CardPlot _plot)
            {
                _eldritch.SetStat(_plot.Eldritch);
            }
        }
    }
}
