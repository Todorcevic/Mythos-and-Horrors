using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using System.Linq;
using Sirenix.Utilities;

namespace MythsAndHorrors.GameView
{
    public class ResourceColorPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is StatGameAction statGameAction)
            {
                Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisStat(statGameAction.Stat);
                if (investigator?.InvestigatorCard.Resources == statGameAction.Stat)
                {
                    ChangeColor(investigator);
                }
            }
            await Task.CompletedTask;
        }

        private void ChangeColor(Investigator investigator)
        {
            _cardViewsManager.GetCardViews(investigator.AllCards)
                .OfType<DeckCardView>()
                .ForEach(deckCardView => deckCardView.ChangeColorResource());
        }
    }
}
