using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01135 : CardAdversityLimbo, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Charge Charge { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Curse, Tag.Isolate };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(1, ChargeType.Special);
        }

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(Charge.Amount, 1).Execute();
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Interactable_Card01135");

            if (investigator.CanPayHints.IsActive)
                interactableGameAction.CreateEffect(this, new Stat(0, false), SpendClue, PlayActionType.Choose, playedBy: investigator, "CardEffect_Card01135");
            interactableGameAction.CreateEffect(this, new Stat(0, false), TakeDamage, PlayActionType.Choose, playedBy: investigator, "CardEffect_Card01135-1");

            await interactableGameAction.Execute();

            /*******************************************************************/
            async Task SpendClue() => await _gameActionsProvider.Create<PayHintGameAction>().SetWith(investigator, Charge.Amount, 1).Execute();
            async Task TakeDamage() => await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountDamage: 2).Execute();
        }
    }
}
