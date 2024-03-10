using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardPlot : Card, IRevellable
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Stat Eldritch { get; private set; }
        public State Revealed { get; private set; }
        public Reaction MustShowInitialHistory { get; private set; }
        public Reaction MustShowFinalHistory { get; private set; }

        /*******************************************************************/
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Eldritch = new Stat(Info.Eldritch ?? 0, Info.Eldritch ?? 0);
            Revealed = new State(false);
            MustShowInitialHistory = new Reaction(CheckShowInitialHistory, ShowInitialHistory);
            MustShowFinalHistory = new Reaction(CanShowFinalHistory, ShowFinalHistory);
        }

        protected override async Task WhenFinish(GameAction gameAction)
        {
            await MustShowInitialHistory.Check(gameAction);
            await MustShowFinalHistory.Check(gameAction);
            await base.WhenFinish(gameAction);
        }

        /********************** SHOW INITIAL HISTORY ****************************/
        protected virtual bool CheckShowInitialHistory(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (Revealed.IsActive) return false;
            if (InitialHistory == null) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.PlotZone) return false;
            return true;
        }

        protected async Task ShowInitialHistory() => await _gameActionFactory.Create(new ShowHistoryGameAction(InitialHistory, this));

        /********************** SHOW FINAL HISTORY *********************/
        protected virtual bool CanShowFinalHistory(GameAction gameAction)
        {
            if (gameAction is not StatGameAction statGameAction) return false;
            if (Revealed.IsActive) return false;
            if (RevealHistory == null) return false;
            if (statGameAction.Stat != Eldritch) return false;
            if (Eldritch.Value < Info.Eldritch) return false;
            return true;
        }

        protected async Task ShowFinalHistory() => await _gameActionFactory.Create(new RevealGameAction(this));
    }
}
