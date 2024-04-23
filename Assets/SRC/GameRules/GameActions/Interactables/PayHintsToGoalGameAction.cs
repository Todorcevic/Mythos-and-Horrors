using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintsToGoalGameAction : GameAction
    {
        private bool _isCancel;
        private IEnumerable<Investigator> _specificInvestigators;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public CardGoal CardGoal { get; }

        private IEnumerable<Investigator> DefaultsInvestigators =>
            _investigatorsProvider.AllInvestigatorsInPlay.Where(investigator => investigator.Hints.Value > 0);
        public IEnumerable<Investigator> InvestigatorsToPay => _specificInvestigators ?? DefaultsInvestigators;
        public override bool CanBeExecuted => !CardGoal.Revealed.IsActive && InvestigatorsToPay.Sum(investigator => investigator.Hints.Value) >= CardGoal.Hints.Value;

        /*******************************************************************/
        public PayHintsToGoalGameAction(CardGoal cardGoal, IEnumerable<Investigator> specificInvestigators = null)
        {
            CardGoal = cardGoal;
            _specificInvestigators = specificInvestigators;
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            while (CanBeExecuted && !_isCancel)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator to pay");

                interactableGameAction.CreateUndoButton().SetLogic(Undo);
                async Task Undo()
                {
                    await _gameActionsProvider.UndoLastInteractable();
                }

                if (CardGoal.Hints.Value == CardGoal.MaxHints)
                {
                    interactableGameAction.CreateMainButton().SetLogic(Cancel);

                    async Task Cancel()
                    {
                        _isCancel = true;
                        await Task.CompletedTask;
                    }
                }

                foreach (Investigator investigator in InvestigatorsToPay)
                {
                    interactableGameAction.Create()
                        .SetCard(investigator.AvatarCard)
                        .SetInvestigator(investigator)
                        .SetCardAffected(CardGoal)
                        .SetLogic(PayHint);

                    /*******************************************************************/
                    async Task PayHint()
                    {
                        int amountHitsToPay = CardGoal.Hints.Value < investigator.Hints.Value ? CardGoal.Hints.Value : investigator.Hints.Value;
                        await _gameActionsProvider.Create(new PayHintGameAction(investigator, CardGoal.Hints, investigator.Hints.Value));
                    }
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
        }
    }
}
