using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization)
        => throw new NotImplementedException();

        public MulliganGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, new Localization("Interactable_Mulligan"));
            ActiveInvestigator = investigator;
            ExecuteSpecificInitialization();
            return this;
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if (IsMainButtonPressed) return;

            await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Execute();
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            CreateContinueMainButton();

            foreach (Card card in ActiveInvestigator.DiscardableCardsInHand)
            {
                CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_Mulligan"));

                /*******************************************************************/
                async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
            }

            //foreach (Card card in ActiveInvestigator.DiscardZone.Cards)
            //{
            //    if (!CanRestore()) continue;

            //    CreateCardEffect(card, new Stat(0, false), Restore, PlayActionType.Choose, ActiveInvestigator, new Localization("CardEffect_Mulligan-1"));

            //    /*******************************************************************/
            //    async Task Restore() => await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, ActiveInvestigator.HandZone).Execute();


            //    bool CanRestore()
            //    {
            //        if (card.HasThisTag(Tag.Weakness)) return false;
            //        return true;
            //    }
            //}
        }
    }
}

