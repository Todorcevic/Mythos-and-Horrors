using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string description)
        => throw new NotImplementedException();

        public MulliganGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, nameof(MulliganGameAction));
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
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Execute();
                }
            }
        }
    }
}

