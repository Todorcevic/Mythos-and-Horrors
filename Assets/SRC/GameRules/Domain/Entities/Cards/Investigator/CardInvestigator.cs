using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardInvestigator : Card, IStartReactionable
    {
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

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
        public Stat Turns { get; private set; }

        public Stat DrawTurnsCost { get; private set; }

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            Health = new Stat(Info.Health ?? 0, Info.Health ?? 0);
            Sanity = new Stat(Info.Sanity ?? 0, Info.Sanity ?? 0);
            Strength = new Stat(Info.Strength ?? 0);
            Agility = new Stat(Info.Agility ?? 0);
            Intelligence = new Stat(Info.Intelligence ?? 0);
            Power = new Stat(Info.Power ?? 0);
            Xp = new Stat(0);
            Injury = new Stat(0);
            Shock = new Stat(0);
            Resources = new Stat(0);
            Hints = new Stat(0);
            Turns = new Stat(0, GameValues.DEFAULT_TURNS_AMOUNT);
            DrawTurnsCost = new Stat(1);

        }

        public bool HasThisStat(Stat stat)
        {
            return stat == Health
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
                || stat == Turns;
        }

        async Task IStartReactionable.WhenBegin(GameAction gameAction)
        {
            CheckDraw(gameAction);
            await Task.CompletedTask;
        }

        /************************** DRAW *****************************/
        protected void CheckDraw(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneTurnGA) return;

            _effectProvider.Create()
                .SetCard(Owner.CardToDraw)
                .SetInvestigator(Owner)
                .SetCanPlay(CanDraw)
                .SetLogic(Draw)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Draw));
        }

        protected bool CanDraw()
        {
            if (_investigatorProvider.ActiveInvestigator != Owner) return false;
            if (Owner.Turns.Value < DrawTurnsCost.Value) return false;

            return true;
        }

        protected async Task Draw()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Owner.Turns, DrawTurnsCost.Value));
            await _gameActionFactory.Create(new DrawGameAction(Owner));
        }
    }
}
