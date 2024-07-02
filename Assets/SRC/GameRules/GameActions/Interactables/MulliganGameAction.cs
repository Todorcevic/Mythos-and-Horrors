using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }

        /*******************************************************************/
        public MulliganGameAction SetWith(Investigator investigator)
        {
            SetWith(canBackToThisInteractable: true, mustShowInCenter: false, nameof(MulliganGameAction));
            ActiveInvestigator = investigator;
            return this;
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
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Start();
                    await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Start();
                }
            }
        }
    }
}

