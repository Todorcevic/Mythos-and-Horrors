using System.Collections.Generic;
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

        private CardPlace Parlor => _cardsProvider.GetCard<Card01115>();
        private CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        private Card Lita => _cardsProvider.GetCard<Card01117>();
        private Card GhoulPriest => _cardsProvider.GetCard<Card01116>();

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new RevealGameAction(Parlor));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Lita, Parlor.OwnZone));
            await _gameActionsProvider.Create(new MoveCardsGameAction(GhoulPriest, Hallway.OwnZone));
        }

        /*******************************************************************/
        public override bool PayHintsConditionToActivate(Investigator investigator) => false;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await OptativeReaction<RoundGameAction>(gameAction, PayHintsCondition, PayHintsLogic);
        }

        /*******************************************************************/
        private bool PayHintsCondition(RoundGameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == Hallway).Sum(investigator => investigator.Hints.Value) < Hints.Value) return false;
            return true;
        }

        private async Task PayHintsLogic(RoundGameAction gameAction)
        {
            IEnumerable<Investigator> specificInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                  .Where(investigator => investigator.CurrentPlace == Hallway && investigator.Hints.Value > 0);

            PayHintsToGoalGameAction payHintsGoalGameAction = new(this, specificInvestigators);
            payHintsGoalGameAction.CreateMainButton().SetLogic(Continue).SetDescription(nameof(Continue));
            await _gameActionsProvider.Create(payHintsGoalGameAction);

            /*******************************************************************/
            static async Task Continue() => await Task.CompletedTask;
        }
    }
}