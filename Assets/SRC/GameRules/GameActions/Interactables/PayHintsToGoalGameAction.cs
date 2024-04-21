using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintsToGoalGameAction : GameAction
    {
        private bool _isCancel;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public CardGoal CardGoal { get; }
        public IEnumerable<Investigator> SpecificInvestigators { get; }

        private IEnumerable<Investigator> DefaultsInvestigators =>
            _investigatorsProvider.AllInvestigatorsInPlay.Where(investigator => investigator.Hints.Value > 0);
        public override bool CanBeExecuted => !CardGoal.Revealed.IsActive || (SpecificInvestigators ?? DefaultsInvestigators).Count() > 0;

        /*******************************************************************/
        public PayHintsToGoalGameAction(CardGoal cardGoal, IEnumerable<Investigator> specificInvestigators = null)
        {
            CardGoal = cardGoal;
            SpecificInvestigators = specificInvestigators;
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
                    _isCancel = true;
                }

                interactableGameAction.CreateMainButton().SetLogic(Continue);
                async Task Continue()
                {
                    _isCancel = true;
                    await Task.CompletedTask;
                }

                foreach (Investigator investigator in SpecificInvestigators ?? DefaultsInvestigators)
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
