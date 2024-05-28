using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckSlotsGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }

        public override bool CanBeExecuted => Investigator.HasSlotsExeded;

        /*******************************************************************/
        public CheckSlotsGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (CanBeExecuted)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Select Supply To Discard", Investigator);
                IEnumerable<CardSupply> cards = Investigator.CardsInPlay.OfType<CardSupply>().Where(card => card.HasAnyOfThisSlots(Investigator.GetAllSlotsExeded()));
                foreach (CardSupply card in cards)
                {
                    interactableGameAction.Create().SetCard(card).SetDescription("Discard").SetLogic(Discard).SetInvestigator(Investigator);

                    async Task Discard()
                    {
                        await _gameActionsProvider.Create(new DiscardGameAction(card));
                        await _gameActionsProvider.Create(new CheckSlotsGameAction(Investigator));
                    }
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
        }
    }
}
