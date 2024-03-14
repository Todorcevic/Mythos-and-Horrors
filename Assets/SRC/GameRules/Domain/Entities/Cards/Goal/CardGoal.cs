using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardGoal : Card, IRevellable
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public Stat Hints { get; private set; }
        public State Revealed { get; private set; }
        public Reaction MustShowInitialHistory { get; private set; }
        public Reaction MustShowFinalHistory { get; private set; }
        public int Position => _chaptersProviders.CurrentScene.Info.GoalCards.IndexOf(this);
        public CardGoal NextCardGoal => _chaptersProviders.CurrentScene.Info.GoalCards.ElementAtOrDefault(Position + 1);

        /*******************************************************************/
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0, Info.Hints ?? 0);
            Revealed = new State(false);
            MustShowInitialHistory = new Reaction(CheckShowInitialHistory, ShowInitialHistory);
            MustShowFinalHistory = new Reaction(CheckShowFinalHistory, Reveal);
        }

        protected override async Task WhenFinish(GameAction gameAction)
        {
            await MustShowInitialHistory.Check(gameAction);
            await MustShowFinalHistory.Check(gameAction);
        }

        /********************** SHOW INITIAL HISTORY ****************************/
        protected virtual bool CheckShowInitialHistory(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (Revealed.IsActive) return false;
            if (InitialHistory == null) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.GoalZone) return false;
            return true;
        }

        protected async Task ShowInitialHistory() => await _gameActionProvider.Create(new ShowHistoryGameAction(InitialHistory, this));

        /********************** SHOW FINAL HISTORY *********************/
        protected virtual bool CheckShowFinalHistory(GameAction gameAction)
        {
            if (gameAction is not StatGameAction statGameAction) return false;
            if (Revealed.IsActive) return false;
            if (RevealHistory == null) return false;
            if (!statGameAction.HasStat(Hints)) return false;
            if (Hints.Value > 0) return false;
            return true;
        }

        protected async Task Reveal() => await _gameActionProvider.Create(new RevealGameAction(this));

        public virtual async Task RevealEffect()
        {
            await _gameActionProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
            await _gameActionProvider.Create(new DiscardGameAction(this));
            await _gameActionProvider.Create(new MoveCardsGameAction(NextCardGoal, _chaptersProviders.CurrentScene.GoalZone));
        }
    }
}
