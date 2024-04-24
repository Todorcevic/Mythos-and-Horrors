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

        public Reaction<RoundGameAction> PayHints { get; private set; }

        private CardPlace Parlor => _cardsProvider.GetCard<Card01115>();
        private CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        private Card Lita => _cardsProvider.GetCard<Card01117>();
        private Card GhoulPriest => _cardsProvider.GetCard<Card01116>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            PayHints = new Reaction<RoundGameAction>(PayHintsCondition, PayHintsLogic);
        }

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new RevealGameAction(Parlor));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Lita, Parlor.OwnZone));
            await _gameActionsProvider.Create(new MoveCardsGameAction(GhoulPriest, Hallway.OwnZone));
        }

        /*******************************************************************/
        public override bool ConditionToActivate(Investigator investigator) => false;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await PayHints.Check(gameAction);
        }
        /*******************************************************************/

        private bool PayHintsCondition(GameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == Hallway).Sum(investigator => investigator.Hints.Value) < Hints.Value) return false;
            return true;
        }

        private async Task PayHintsLogic(GameAction gameAction)
        {
            IEnumerable<Investigator> specificInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                  .Where(investigator => investigator.CurrentPlace == Hallway && investigator.Hints.Value > 0);
            await _gameActionsProvider.Create(new PayHintsToGoalGameAction(this, specificInvestigators));
        }
    }
}