using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator) :
            base(canBackToThisInteractable: true, mustShowInCenter: false, "Mulligan", investigator)
        { }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            CreateContinueMainButton();

            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                Create(card, Discard, PlayActionType.Choose, ActiveInvestigator);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new MulliganGameAction(ActiveInvestigator));
                }
            }
        }
    }
}

