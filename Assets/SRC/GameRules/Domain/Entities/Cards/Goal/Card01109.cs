using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01109 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        private CardPlace Parlor => _cardsProvider.GetCard<Card01115>();
        private CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        private Card Lita => _cardsProvider.GetCard<Card01117>();
        private Card GhoulPriest => _cardsProvider.GetCard<Card01116>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            CreateOptativeReaction<RoundGameAction>(PayHintsCondition, PayHintsLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new RevealGameAction(Parlor));
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Lita, Parlor.OwnZone).Start();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(GhoulPriest, Hallway.OwnZone).Start();
        }

        /*******************************************************************/
        private bool PayHintsCondition(RoundGameAction roundGameAction)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == Hallway).Sum(investigator => investigator.Hints.Value) < Hints.Value) return false;
            return true;
        }

        private async Task PayHintsLogic(RoundGameAction roundGameAction)
        {
            IEnumerable<Investigator> specificInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                  .Where(investigator => investigator.CurrentPlace == Hallway && investigator.Hints.Value > 0);

            await _gameActionsProvider.Create<PayHintsToGoalGameAction>().SetWith(this, specificInvestigators).Start();
        }

        protected override bool PayHintsConditionToActivate(Investigator investigator) => false;
    }
}