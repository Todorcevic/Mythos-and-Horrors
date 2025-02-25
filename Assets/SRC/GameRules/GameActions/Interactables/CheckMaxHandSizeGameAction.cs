using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }
        public IEnumerable<Card> DiscardableCards { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization) => throw new NotImplementedException();

        public CheckMaxHandSizeGameAction SetWith(Investigator investigator, IEnumerable<Card> discardableCards)
        {
            ActiveInvestigator = investigator;
            DiscardableCards = discardableCards;
            base.SetWith(canBackToThisInteractable: false, mustShowInCenter: false, new Localization("Interactable_CheckMaxHandSize", DescriptionParams()));
            ExecuteSpecificInitialization();
            return this;

            /*******************************************************************/
            string[] DescriptionParams()
            {
                int cardsLeft = investigator.HandSize - investigator.MaxHandSize.Value;
                return new[] { investigator.InvestigatorCard.Info.Name, investigator.MaxHandSize.Value.ToString(), cardsLeft.ToString() };
            }
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if (IsUndoPressed || IsMainButtonPressed) return;

            await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(ActiveInvestigator, DiscardableCards).Execute();
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            if (ActiveInvestigator.HandSize <= ActiveInvestigator.MaxHandSize.Value) CreateContinueMainButton();
            else CreateDiscardCards();
            CreateRestoreCards();
        }

        /*******************************************************************/
        private void CreateDiscardCards()
        {
            foreach (Card card in ActiveInvestigator.DiscardableCardsInHand.Where(card => DiscardableCards.Contains(card)))
            {
                CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_CheckMaxHandSize_Discard"));

                /*******************************************************************/
                async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
            }
        }

        private void CreateRestoreCards()
        {
            foreach (Card card in ActiveInvestigator.DiscardZone.Cards.Where(card => DiscardableCards.Contains(card)))
            {
                CreateCardEffect(card, new Stat(0, false), Restore, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_CheckMaxHandSize_Restore"));

                /*******************************************************************/
                async Task Restore() => await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, ActiveInvestigator.HandZone).Execute();
            }
        }
    }
}
