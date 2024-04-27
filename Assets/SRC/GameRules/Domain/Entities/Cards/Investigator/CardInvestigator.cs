using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardInvestigator : Card, IDamageable, IFearable
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
        public Reaction<UpdateStatGameAction> Defeat { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            Health = new Stat(Info.Health ?? 0 - Injury.Value);
            Sanity = new Stat(Info.Sanity ?? 0 - Shock.Value);
            Strength = new Stat(Info.Strength ?? 0);
            Agility = new Stat(Info.Agility ?? 0);
            Intelligence = new Stat(Info.Intelligence ?? 0);
            Power = new Stat(Info.Power ?? 0);
            Xp = new Stat(0);
            Injury = new Stat(0);
            Shock = new Stat(0);
            Resources = new Stat(0);
            Hints = new Stat(0);
            CurrentTurns = new Stat(GameValues.DEFAULT_TURNS_AMOUNT);
            MaxHandSize = new Stat(GameValues.MAX_HAND_SIZE);
            MaxTurns = new Stat(GameValues.DEFAULT_TURNS_AMOUNT);
            DrawTurnsCost = new Stat(1);
            TurnsCost = new Stat(1);
            Resign = new State(false);
            Defeated = new State(false);
            Defeat = new Reaction<UpdateStatGameAction>(DefeatCondition, DefeatLogic);
        }

        /*******************************************************************/
        public bool HasThisStat(Stat stat) => stat == Health
                || stat == Sanity
                || stat == Strength
                || stat == Agility
                || stat == Intelligence
                || stat == Power
                || stat == Xp
                || stat == Injury
                || stat == Shock
                || stat == Resources
                || stat == Hints
                || stat == CurrentTurns;

        public abstract Task StarEffect();

        public abstract int StarValue();

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await Defeat.Check(gameAction);
        }

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
    }
}
