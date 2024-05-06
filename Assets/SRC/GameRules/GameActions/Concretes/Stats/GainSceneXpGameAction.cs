using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainSceneXpGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        private IEnumerable<CardPlace> PlaceCardsWithXP => _chaptersProvider.CurrentScene.Info.PlaceCards
         .Where(cardPlace => cardPlace.IsVictory && cardPlace.IsInPlay && cardPlace.Revealed.IsActive && cardPlace.Hints.Value < 1);

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            int amountXp = _chaptersProvider.CurrentScene.VictoryZone.Cards.Concat(PlaceCardsWithXP).Sum(card => card.Info.Victory) ?? 0;
            Dictionary<Stat, int> xp = _investigatorsProvider.AllInvestigators.ToDictionary(investigator => investigator.Xp, investigator => amountXp);

            foreach (var investigator in _investigatorsProvider.AllInvestigators)
            {
                int individualAmountXp = _chaptersProvider.CurrentScene.VictoryZone.Cards.OfType<IVictoriable>()
                    .Where(victoriable => victoriable.InvestigatorsVictoryAffected.Contains(investigator)).Sum(victoriable => victoriable.Victory.Value);
                xp[investigator.Xp] += individualAmountXp;
            }

            await _gameActionsProvider.Create(new IncrementStatGameAction(xp));
        }
    }
}
