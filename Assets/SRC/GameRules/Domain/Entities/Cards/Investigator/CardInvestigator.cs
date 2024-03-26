using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardInvestigator : Card
    {
        public int RealHealth => Info.Health ?? 0 - Injury.Value;
        public int RealSanity => Info.Sanity ?? 0 - Shock.Value;
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
        public Stat ResourceTurnsCost { get; private set; }
        public ChallengeToken StarToken { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            Health = new Stat(RealHealth);
            Sanity = new Stat(RealSanity);
            Strength = new Stat(Info.Strength ?? 0);
            Agility = new Stat(Info.Agility ?? 0);
            Intelligence = new Stat(Info.Intelligence ?? 0);
            Power = new Stat(Info.Power ?? 0);
            Xp = new Stat(0);
            Injury = new Stat(0);
            Shock = new Stat(0);
            Resources = new Stat(0);
            Hints = new Stat(0);
            CurrentTurns = new Stat(0);
            MaxHandSize = new Stat(GameValues.MAX_HAND_SIZE);
            MaxTurns = new Stat(GameValues.DEFAULT_TURNS_AMOUNT);
            DrawTurnsCost = new Stat(1);
            ResourceTurnsCost = new Stat(1);
            StarToken = new ChallengeToken();
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
    }
}
