using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardInvestigator : Card
    {
        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat Intelligence { get; private set; }
        public Stat Power { get; private set; }

        public Stat DeckSize { get; private set; }
        public Stat Xp { get; private set; }
        public Stat Injury { get; private set; }
        public Stat Shock { get; private set; }
        public Stat Resources { get; private set; }
        public Stat Hints { get; private set; }
        public Stat InitialHandSize { get; private set; }
        public Stat Turns { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Health = new Stat(Info.Health ?? 0, Info.Health ?? 0);
            Sanity = new Stat(Info.Sanity ?? 0, Info.Sanity ?? 0);
            Strength = new Stat(Info.Strength ?? 0);
            Agility = new Stat(Info.Agility ?? 0);
            Intelligence = new Stat(Info.Intelligence ?? 0);
            Power = new Stat(Info.Power ?? 0);

            DeckSize = new Stat(30);
            Xp = new Stat(0);
            Injury = new Stat(0);
            Shock = new Stat(0);
            Resources = new Stat(0);
            Hints = new Stat(0);
            InitialHandSize = new Stat(5);
            Turns = new Stat(0, 3);
        }

        public bool HasThisStat(Stat stat)
        {
            return stat == Health
                || stat == Sanity
                || stat == Strength
                || stat == Agility
                || stat == Intelligence
                || stat == Power
                || stat == DeckSize
                || stat == Xp
                || stat == Injury
                || stat == Shock
                || stat == Resources
                || stat == Hints
                || stat == InitialHandSize
                || stat == Turns;
        }
    }
}
