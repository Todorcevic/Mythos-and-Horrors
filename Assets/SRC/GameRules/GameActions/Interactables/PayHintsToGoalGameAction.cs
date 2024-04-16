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
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override bool CanBeExecuted => !_chaptersProvider.CurrentScene.CurrentGoal.Revealed.IsActive;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            while (CanBeExecuted && !_isCancel)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator to pay");
                interactableGameAction.CreateMainButton().SetLogic(Undo);
                async Task Undo()
                {
                    await interactableGameAction.UndoEffect.Logic.Invoke();
                    _isCancel = true;
                }

                foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay
                    .Where(investigator => investigator.Hints.Value > 0))
                {
                    interactableGameAction.Create()
                        .SetCard(investigator.AvatarCard)
                        .SetInvestigator(investigator)
                        .SetCardAffected(_chaptersProvider.CurrentScene.CurrentGoal)
                        .SetLogic(PayHint);

                    /*******************************************************************/
                    async Task PayHint()
                    {
                        await _gameActionsProvider.Create(new PayHintGameAction(investigator, _chaptersProvider.CurrentScene.CurrentGoal.Hints, 1));
                    }
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
        }
    }
}
