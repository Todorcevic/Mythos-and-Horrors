using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckSlotsGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator ActiveInvestigator { get; }
        public override bool CanBeExecuted => ActiveInvestigator.HasSlotsExeded;

        /*******************************************************************/
        public CheckSlotsGameAction(Investigator investigator) : base(canBackToThisInteractable: true, mustShowInCenter: true, "Select Supply To Discard")
        {
            ActiveInvestigator = investigator;
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
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new CheckSlotsGameAction(ActiveInvestigator));
                }
            }
        }
    }
}
