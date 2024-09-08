using System;
using System.Diagnostics.CodeAnalysis;
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
        public Stat Resources { get; private set; }
        public Stat Keys { get; private set; }
        public Stat CurrentActions { get; private set; }
        public Stat MaxActions { get; private set; }
        public Stat MaxHandSize { get; private set; }
        public State Resign { get; private set; }
        public State Defeated { get; private set; }
        public State IsPlaying { get; private set; }
        public State Isolated { get; private set; }
        public Conditional CanPayKeys { get; private set; }
        public Conditional CanBeHealed { get; private set; }
        public Conditional CanBeRestoreSanity { get; private set; }
        public Conditional CanPlayCardsFromHand { get; private set; }
        public Conditional CanMove { get; private set; }
        public Conditional CanInvestigate { get; private set; }
        public Conditional CanAttack { get; private set; }
        public Conditional CanElude { get; internal set; }
        public Conditional CanConfornt { get; internal set; }
        public Conditional HasActionsAvailable { get; private set; }
        public Func<int> StarTokenValue { get; private set; }
        public Func<Task> StarTokenEffect { get; private set; }
        public Func<string> StarTokenDescription { get; protected set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            DamageRecived = CreateStat(0);
            FearRecived = CreateStat(0);
            Strength = CreateStat(Info.Strength ?? 0);
            Agility = CreateStat(Info.Agility ?? 0);
            Intelligence = CreateStat(Info.Intelligence ?? 0);
            Power = CreateStat(Info.Power ?? 0);
            Resources = CreateStat(0);
            Keys = CreateStat(0);
            MaxActions = CreateStat(GameValues.DEFAULT_ACTIONS_AMOUNT);
            CurrentActions = CreateStat(GameValues.DEFAULT_ACTIONS_AMOUNT);
            MaxHandSize = CreateStat(GameValues.MAX_HAND_SIZE);
            Resign = CreateState(false, isReseteable: false);
            Defeated = CreateState(false, isReseteable: false);
            IsPlaying = CreateState(false);
            Isolated = CreateState(false);
            CanPayKeys = new Conditional(() => Keys.Value > 0);
            CanBeHealed = new Conditional(() => DamageRecived.Value > 0);
            CanBeRestoreSanity = new Conditional(() => FearRecived.Value > 0);
            HasActionsAvailable = new Conditional(() => CurrentActions.Value > 0);
            IsInPlay = new Conditional(() => CurrentZone.ZoneType == ZoneType.Investigator);
            StarTokenValue = StarValue;
            StarTokenEffect = StarEffect;
            StarTokenDescription = () => ExtraInfo.StarTokenDescription;

            CreateBaseReaction<MoveCardsGameAction>(CheckSlotsCondition, CheckSlotsLogic, GameActionTime.After);
        }

        public void SetLazyStats(int injuryValue, int shockValue)
        {
            Health = CreateStat(Info.Health ?? 0 - injuryValue);
            Sanity = CreateStat(Info.Sanity ?? 0 - shockValue);

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
