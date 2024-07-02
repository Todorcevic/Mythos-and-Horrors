using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CheckSlotsGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.HasSlotsExeded;

        /*******************************************************************/
        public CheckSlotsGameAction SetWith(Investigator investigator)
        {
            SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Select Supply To Discard");
            ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            IEnumerable<CardSupply> cards = ActiveInvestigator.CardsInPlay.OfType<CardSupply>()
                .Where(card => card.HasAnyOfThisSlots(ActiveInvestigator.GetAllSlotsExeded()));
            foreach (CardSupply card in cards)
            {
                CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator);

                async Task Discard()
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<CheckSlotsGameAction>().SetWith(ActiveInvestigator).Execute();
                }
            }
        }
    }
}
