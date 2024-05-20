using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction, IInitializable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator) :
            base(canBackToThisInteractable: true, mustShowInCenter: false, "Mulligan")
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        public void ExecuteSpecificInitialization()
        {
            CreateMainButton().SetLogic(Continue);

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

            /*******************************************************************/
            async Task Continue() => await Task.CompletedTask;
        }
    }
}

