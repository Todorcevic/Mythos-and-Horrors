using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01135 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Curse, Tag.Isolate };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraStat = CreateStat(0);
        }
        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Choose", investigator);
            if (investigator.Hints.Value > 0)
                interactableGameAction.Create(this, SpendClue, PlayActionType.Choose, playedBy: investigator);
            interactableGameAction.Create(this, TakeDamage, PlayActionType.Choose, playedBy: investigator);
            await _gameActionsProvider.Create(interactableGameAction);

            async Task SpendClue()
            {
                await _gameActionsProvider.Create(new DropHintGameAction(investigator, ExtraStat, 1));
            }

            async Task TakeDamage()
            {
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountDamage: 2));
            }
        }
    }
}
