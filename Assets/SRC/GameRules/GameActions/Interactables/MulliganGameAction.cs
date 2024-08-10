using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction, IPersonalInteractable
    {
        private const string CODE = "Mulligan";

        public Investigator ActiveInvestigator { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string code, params string[] args)
        => throw new NotImplementedException();

        public MulliganGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, code: CODE);
            ActiveInvestigator = investigator;
            ExecuteSpecificInitialization();
            return this;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            CreateContinueMainButton();

            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Execute();
                }
            }

            foreach (Card card in ActiveInvestigator.DiscardZone.Cards)
            {
                if (!CanRestore()) continue;

                CreateEffect(card, new Stat(0, false), Restore, PlayActionType.Choose, ActiveInvestigator);

                /*******************************************************************/
                async Task Restore()
                {
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, ActiveInvestigator.HandZone).Execute();
                    await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Execute();
                }

                bool CanRestore()
                {
                    if (card.HasThisTag(Tag.Weakness)) return false;
                    return true;
                }
            }
        }
    }
}

