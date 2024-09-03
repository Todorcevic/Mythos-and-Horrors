using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CheckSlotsGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.HasSlotsExeded;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization) => throw new NotImplementedException();

        public CheckSlotsGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: true, new Localization("Interactable_CheckSlots", DescriptionParams()));
            ActiveInvestigator = investigator;
            ExecuteSpecificInitialization();
            return this;

            /*******************************************************************/
            string DescriptionParams()
            {
                string slots = string.Empty;
                investigator.GetAllSlotsExeded().Distinct().ForEach(slot => slots += slot.ToString() + "-");
                return slots.Length > 0 ? slots.Remove(slots.Length - 1) : slots;
            }
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            IEnumerable<CardSupply> cards = ActiveInvestigator.CardsInPlay.OfType<CardSupply>()
                .Where(card => card.HasAnyOfThisSlots(ActiveInvestigator.GetAllSlotsExeded()));
            foreach (CardSupply card in cards)
            {
                CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_CheckSlots"));

                async Task Discard()
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<CheckSlotsGameAction>().SetWith(ActiveInvestigator).Execute();
                }
            }
        }
    }
}
