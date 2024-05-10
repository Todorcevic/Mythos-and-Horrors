using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PlotCardView : CardView
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        [Title(nameof(PlotCardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private EldritchStatView _eldritch;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            if (Card is CardPlot _plot)
            {
                _eldritch.SetStat(_plot.Eldritch, _cardsProvider.AllCards.OfType<IEldritchable>()
                    .Select(eldritchStat => eldritchStat.Eldritch).ToList());
            }
        }
    }
}
