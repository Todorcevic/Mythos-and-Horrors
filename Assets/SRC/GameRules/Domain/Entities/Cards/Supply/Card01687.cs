using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01687 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Tool, Tag.Illicit };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.Supplie);
            CreateFastActivation(InvestigationLogic, InvestigationCondition, PlayActionType.Activate);
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private bool InvestigationCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task InvestigationLogic(Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
               .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Place");
            interactable.CreateEffect(investigator.CurrentPlace, investigator.CurrentPlace.InvestigationTurnsCost, Investigate, PlayActionType.Investigate, investigator, cardAffected: this);
            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
                InvestigatePlaceGameAction investigatePlaceGameAction = _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                    .SetWith(investigator, investigator.CurrentPlace);
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigatePlaceGameAction.StatModifier, investigator.Agility.Value).Execute();
                await investigatePlaceGameAction.Execute();

                if (investigatePlaceGameAction.ResultChallenge.TotalDifferenceValue < 2)
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
            }
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (!updateStatGameAction.HasThisStat(Charge.Amount)) return false;
            if (Charge.Amount.Value > 0) return false;
            return true;
        }
    }
}
