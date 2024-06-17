using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator ActiveInvestigator { get; }

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator) : base(canBackToThisInteractable: true, mustShowInCenter: false, "Mulligan")
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            CreateContinueMainButton();

            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator);

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

