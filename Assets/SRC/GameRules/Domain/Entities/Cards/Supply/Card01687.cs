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
            CreateActivation(1, InvestigationLogic, InvestigationCondition, PlayActionType.Investigate);
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        private bool InvestigationCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task InvestigationLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
            InvestigatePlaceGameAction investigatePlaceGameAction = new(investigator, investigator.CurrentPlace);
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigatePlaceGameAction.StatModifier, investigator.Agility.Value).Start();
            await _gameActionsProvider.Create(investigatePlaceGameAction);
            if (investigatePlaceGameAction.ResultChallenge.TotalDifferenceValue < 2)
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
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
