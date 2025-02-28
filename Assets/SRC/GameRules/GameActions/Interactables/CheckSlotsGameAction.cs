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
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue && ActiveInvestigator.HasSlotsExeded;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization) => throw new NotImplementedException();

        public CheckSlotsGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_CheckSlots", DescriptionParams()));
            ActiveInvestigator = investigator;
            ExecuteSpecificInitialization();
            return this;

            /*******************************************************************/
            string DescriptionParams()
            {
                return string.Join("-", investigator.GetAllSlotsExeded().Distinct());
            }
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if (IsUndoPressed) return;
            await _gameActionsProvider.Create<CheckSlotsGameAction>().SetWith(ActiveInvestigator).Execute(); ;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            IEnumerable<CardSupply> cards = ActiveInvestigator.CardsInPlay.OfType<CardSupply>()
                .Where(card => card.HasAnyOfThisSlots(ActiveInvestigator.GetAllSlotsExeded()));
            foreach (CardSupply card in cards)
            {
                CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_CheckSlots"));

                /*******************************************************************/
                async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
            }
        }
    }
}
