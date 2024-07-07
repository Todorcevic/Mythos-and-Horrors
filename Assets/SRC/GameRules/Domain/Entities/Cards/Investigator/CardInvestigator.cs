using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardInvestigator : Card, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat FearRecived { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat Intelligence { get; private set; }
        public Stat Power { get; private set; }
        public Stat Xp { get; private set; }
        public Stat Injury { get; private set; }
        public Stat Shock { get; private set; }
        public Stat Resources { get; private set; }
        public Stat Hints { get; private set; }
        public Stat CurrentTurns { get; private set; }
        public Stat MaxTurns { get; private set; }
        public Stat MaxHandSize { get; private set; }
        public Stat DrawTurnsCost { get; private set; }
        public Stat TurnsCost { get; private set; }
        public State Resign { get; private set; }
        public State Defeated { get; private set; }
        public State IsPlaying { get; private set; }
        public State Isolated { get; private set; }
        public Conditional CanPayHints { get; private set; }
        public Func<int> StarTokenValue { get; private set; }
        public Func<Task> StarTokenEffect { get; private set; }
        public override bool IsInPlay => CurrentZone.ZoneType == ZoneType.Investigator;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0 - Injury.Value);
            Sanity = CreateStat(Info.Sanity ?? 0 - Shock.Value);
            DamageRecived = CreateStat(0);
            FearRecived = CreateStat(0);
            Strength = CreateStat(Info.Strength ?? 0);
            Agility = CreateStat(Info.Agility ?? 0);
            Intelligence = CreateStat(Info.Intelligence ?? 0);
            Power = CreateStat(Info.Power ?? 0);
            Xp = CreateStat(0);
            Injury = CreateStat(0);
            Shock = CreateStat(0);
            Resources = CreateStat(0);
            Hints = CreateStat(0);
            MaxTurns = CreateStat(GameValues.DEFAULT_TURNS_AMOUNT);
            CurrentTurns = CreateStat(GameValues.DEFAULT_TURNS_AMOUNT);
            MaxHandSize = CreateStat(GameValues.MAX_HAND_SIZE);
            DrawTurnsCost = CreateStat(1);
            TurnsCost = CreateStat(1);
            Resign = CreateState(false, isReseteable: false);
            Defeated = CreateState(false, isReseteable: false);
            IsPlaying = CreateState(false);
            Isolated = CreateState(false);
            CanPayHints = new Conditional(() => Hints.Value > 0);
            StarTokenValue = StarValue;
            StarTokenEffect = StarEffect;
            CreateBaseReaction<MoveCardsGameAction>(CheckSlotsCondition, CheckSlotsLogic, GameActionTime.After);
            CreateBaseReaction<MoveCardsGameAction>(CheckRestoreDeckCondition, CheckRestoreDeckLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task CheckRestoreDeckLogic(MoveCardsGameAction action)
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
              .SetWith(Owner.DiscardZone.Cards, Owner.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(Owner.DeckZone).Execute();
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(Owner, null, amountFear: 1).Execute();
        }

        private bool CheckRestoreDeckCondition(MoveCardsGameAction action)
        {
            if (!IsInPlay) return false;
            if (Owner.DeckZone.Cards.Any()) return false;
            if (!Owner.DiscardZone.Cards.Any()) return false;
            return true;
        }

        /*******************************************************************/
        protected virtual Task StarEffect() => Task.CompletedTask;
        protected virtual int StarValue() => 0;

        /*******************************************************************/
        private bool CheckSlotsCondition(MoveCardsGameAction action)
        {
            if (!Owner.HasSlotsExeded) return false;
            return true;
        }

        private async Task CheckSlotsLogic(MoveCardsGameAction action)
        {
            await _gameActionsProvider.Create<CheckSlotsGameAction>().SetWith(Owner).Execute();
        }

        /*******************************************************************/
        protected override void BlankState(bool isActive)
        {
            base.BlankState(isActive);
            if (isActive)
            {
                StarTokenValue = () => 0;
                StarTokenEffect = () => Task.CompletedTask;
            }
            else
            {
                StarTokenValue = StarValue;
                StarTokenEffect = StarEffect;
            }
        }

        /*******************************************************************/
        public async Task Reset()
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(DamageRecived, DamageRecived.InitialValue).Execute();
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(FearRecived, FearRecived.InitialValue).Execute();
        }
    }
}
