using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }

        public override bool CanBeExecuted => ActiveInvestigator.HandSize > ActiveInvestigator.MaxHandSize.Value;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization) => throw new NotImplementedException();

        public CheckMaxHandSizeGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, new Localization("Interactable_CheckMaxHandSize", DescriptionParams()));
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
        private void ExecuteSpecificInitialization()
        {
            foreach (Card card in ActiveInvestigator.DiscardableCardsInHand)
            {
                CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_CheckMaxHandSize_Discard"));

                /*******************************************************************/
                async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
            }
        }
    }
}
