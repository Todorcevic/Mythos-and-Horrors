using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01510 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        public State Protected { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandTurnsCost = CreateStat(0);
            Protected = CreateState(false);
            CreateReaction<PlayInvestigatorGameAction>(PlayFromHandCondition.Result, PlayFromHandReactionLogic, isAtStart: true, isOptative: true);
            CreateReaction<RoundGameAction>(RemovePlayedCondition, RemovePlayedLogic, isAtStart: true);
            CreateReaction<CreatureAttackGameAction>(CancelAttackCreatureCondition, CancelAttackCreaturePlayedLogic, isAtStart: true);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff);
        }

        /*******************************************************************/
        private async Task DeactivationBuff(IEnumerable<Card> enumerable)
        {
            await Task.CompletedTask;
        }

        private async Task ActivationBuff(IEnumerable<Card> enumerable)
        {
            await Task.CompletedTask;
        }

        private IEnumerable<Card> CardsToBuff() =>
            Protected.IsActive ? new List<CardInvestigator>() { Owner.InvestigatorCard } : Enumerable.Empty<Card>();

        /*******************************************************************/
        private async Task CancelAttackCreaturePlayedLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            creatureAttackGameAction.Cancel();
            await Task.CompletedTask;
        }

        private bool CancelAttackCreatureCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (!Protected.IsActive) return false;
            if (creatureAttackGameAction.Investigator != ControlOwner) return false;
            if (creatureAttackGameAction.Creature.HasThisTag(Tag.Elite)) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RemovePlayedLogic(RoundGameAction action)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Protected, false));
        }

        private bool RemovePlayedCondition(RoundGameAction action)
        {
            if (!Protected.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        public async Task PlayFromHandReactionLogic(GameAction gameAction)
        {
            await _gameActionsProvider.Create(new PlayFromHandGameAction(this, ControlOwner));
        }

        public override bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (gameAction is not PlayInvestigatorGameAction playInvestigatorGameAction) return false;
            if (playInvestigatorGameAction.ActiveInvestigator != ControlOwner) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner.Resources.Value < ResourceCost.Value) return false;
            return true;
        }

        /*******************************************************************/
        public override async Task ExecuteConditionEffect()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Protected, true));
        }
    }
}
