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
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Choose");

            if (investigator.Hints.Value > 0)
                interactableGameAction.CreateEffect(this, new Stat(0, false), SpendClue, PlayActionType.Choose, playedBy: investigator);
            interactableGameAction.CreateEffect(this, new Stat(0, false), TakeDamage, PlayActionType.Choose, playedBy: investigator);

            await interactableGameAction.Start();

            /*******************************************************************/
            async Task SpendClue() => await _gameActionsProvider.Create<DropHintGameAction>().SetWith(investigator, ExtraStat, 1).Start();
            async Task TakeDamage() => await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountDamage: 2).Start();
        }
    }
}
