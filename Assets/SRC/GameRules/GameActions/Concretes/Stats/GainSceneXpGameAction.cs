using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainSceneXpGameAction : GameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        private IEnumerable<CardPlace> PlaceCardsWithXP => _chaptersProvider.CurrentScene.PlaceCards
         .Where(cardPlace => cardPlace.IsVictory && cardPlace.IsInPlay && cardPlace.Revealed.IsActive && cardPlace.Hints.Value < 1);

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            int amountXp = _chaptersProvider.CurrentScene.VictoryZone.Cards.Concat(PlaceCardsWithXP).Sum(card => card.Info.Victory) ?? 0;
            Dictionary<Stat, int> xp = _investigatorsProvider.AllInvestigators
                .ToDictionary(investigator => investigator.Xp, investigator => amountXp);

            foreach (IVictoriable victoriable in _cardsProvider.AllCards.OfType<IVictoriable>()
                .Where(victoriable => victoriable.IsVictoryComplete))
            {
                victoriable.InvestigatorsVictoryAffected.ForEach(investigator => xp[investigator.Xp] += victoriable.Victory);
            }
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(xp).Execute();
        }
    }
}
