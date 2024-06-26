using ModestTree;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01558 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };
        public Stat AmountCharges { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountCharges = CreateStat(4);
            CreateFastActivation(Logic, Condition, PlayActionType.Activate);
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (!updateStatGameAction.HasThisStat(AmountCharges)) return false;
            if (AmountCharges.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
            await _gameActionsProvider.Create(new DecrementStatGameAction(AmountCharges, 1));
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ControlOwner, fromCard: this, amountFear: 1));
            await _gameActionsProvider.Create(new GainResourceGameAction(ControlOwner, 1));
        }
    }
}
