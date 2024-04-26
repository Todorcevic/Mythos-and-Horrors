using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator) :
            base(canBackToThisInteractable: true, mustShowInCenter: false, "Mulligan")
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected sealed override async Task ExecuteThisLogic()
        {
            CreateMainButton().SetLogic(Continue);
            CreateUndoButton().SetLogic(UndoEffect);


            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                Create().SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetLogic(Discard);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new MulliganGameAction(ActiveInvestigator));
                }
            }


            await base.ExecuteThisLogic();

            /*******************************************************************/

            async Task Continue() => await Task.CompletedTask;

            async Task UndoEffect()
            {
                InteractableGameAction lastInteractable = await _gameActionsProvider.UndoLastInteractable();
                lastInteractable.ClearEffects();
                await _gameActionsProvider.Create(lastInteractable);
            }
        }
    }
}

