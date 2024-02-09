using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using System.Linq;
using Sirenix.Utilities;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class ResourceColorPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is MoveCardsGameAction moveCardsGameAction)
            {
                ChangeColor(moveCardsGameAction.CardsInOutHand);
                return;
            }

            if (gameAction is StatGameAction statGameAction)
            {
                Investigator investigator = _investigatorsProvider.GetInvestigatorWithThisStat(statGameAction.Stat);
                if (investigator?.InvestigatorCard.Resources == statGameAction.Stat) ChangeColor(investigator.AllCards);
                return;

            }
            await Task.CompletedTask;
        }
        private void ChangeColor(List<Card> cards)
        {
            _cardViewsManager.GetCardViews(cards)
                .OfType<DeckCardView>().Where(deckCardView => deckCardView.HasResource)
                .ForEach(deckCardView => deckCardView.ChangeColorResource());
        }
    }
}
