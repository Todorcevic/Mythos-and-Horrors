using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : InteractableGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Investigator InvestigatorSelected { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization)
         => throw new NotImplementedException();

        public ChooseInvestigatorGameAction SetWith()
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: true, new Localization("Interactable_ChooseInvestigator"));
            ExecuteSpecificInitialization();
            return this;
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if (IsUndoPressed)
                InvestigatorSelected = _investigatorsProvider.AllInvestigatorsInPlay.FirstOrDefault(investigator => investigator.IsPlayingHisTurn.IsActive);
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStartTurn)
            {
                CreateCardEffect(investigator.AvatarCard, new Stat(0, false), PlayInvestigator, PlayActionType.Choose, investigator, new Localization("CardEffect_ChooseInvestigator"));

                /*******************************************************************/
                async Task PlayInvestigator()
                {
                    InvestigatorSelected = investigator;
                    await Task.CompletedTask;
                }
            }
        }
    }
}
