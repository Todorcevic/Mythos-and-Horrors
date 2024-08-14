using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01587 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Tool };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.Supplie);
            CreateFastActivation(InvestigateLogic, InvestigateCondition, PlayActionType.Activate, "Activation_Card01587");
        }

        /*******************************************************************/
        private bool InvestigateCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Charge.IsEmpty) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CanInvestigate) return false;
            if (!investigator.CurrentPlace.CanBeInvestigated.IsActive) return false;
            return true;
        }

        private async Task InvestigateLogic(Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
               .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, localizableCode: "Interactable_Card01587");
            interactable.CreateEffect(investigator.CurrentPlace, investigator.CurrentPlace.InvestigationTurnsCost, Investigate,
                PlayActionType.Investigate, investigator, "CardEffect_Card01587", cardAffected: this);
            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(investigator.CurrentPlace.Enigma, 2).Execute();
                await _gameActionsProvider.Create<InvestigatePlaceGameAction>().SetWith(investigator, investigator.CurrentPlace).Execute();
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigator.CurrentPlace.Enigma, 2).Execute();
            }
        }
    }
}
