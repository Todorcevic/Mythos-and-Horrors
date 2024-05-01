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
        public IReaction DefeatReaction { get; private set; }
        public Func<int> StarTokenValue { get; private set; }
        public Func<Task> StarTokenEffect { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0 - Injury.Value);
            Sanity = CreateStat(Info.Sanity ?? 0 - Shock.Value);
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
            Resign = new State(false);
            Defeated = new State(false);
            DefeatReaction = CreateReaction<UpdateStatGameAction>(DefeatCondition, DefeatLogic, false);
            StarTokenValue = StarValue;
            StarTokenEffect = StarEffect;
        }

        /*******************************************************************/
        protected virtual Task StarEffect() => Task.CompletedTask;

        protected virtual int StarValue() => 0;

        /*******************************************************************/
        private bool DefeatCondition(UpdateStatGameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (!DieByDamage() && !DieByFear()) return false;
            return true;

            bool DieByDamage()
            {
                if (this is not IDamageable damageable) return false;
                if (damageable.Health.Value > 0) return false;
                return true; ;
            }

            bool DieByFear()
            {
                if (this is not IFearable fearable) return false;
                if (fearable.Sanity.Value > 0) return false;
                return true;
            }
        }

        private async Task DefeatLogic(UpdateStatGameAction gameAction)
        {
            Card byThisCard = null;
            if (gameAction.Parent is HarmToCardGameAction harmToCardGameAction) byThisCard = harmToCardGameAction.ByThisCard;

            await _gameActionsProvider.Create(new DefeatCardGameAction(this, byThisCard));
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
    }
}
