using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardPlot : Card, IEndReactionable, IRevellable
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Stat Eldritch { get; private set; }
        public State Revealed { get; private set; }
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Eldritch = new Stat(Info.Eldritch ?? 0, Info.Eldritch ?? 0);
            Revealed = new State(false);
        }

        /*******************************************************************/
        async Task IEndReactionable.WhenFinish(GameAction gameAction)
        {
            if (gameAction is MoveCardsGameAction moveCardsGameAction) await CanShowInitialHistory(moveCardsGameAction);
            if (gameAction is StatGameAction statGameAction) await CanShowFinalHistory(statGameAction);
        }

        protected virtual async Task CanShowInitialHistory(MoveCardsGameAction moveCardsGameAction)
        {
            if (Revealed.IsActive) return;
            if (InitialHistory == null) return;
            if (!moveCardsGameAction.Cards.Contains(this)) return;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.PlotZone) return;

            await _gameActionFactory.Create(new ShowHistoryGameAction(InitialHistory, this));
        }

        protected virtual async Task CanShowFinalHistory(StatGameAction statGameAction)
        {
            if (Revealed.IsActive) return;
            if (RevealHistory == null) return;
            if (statGameAction.Stat != Eldritch) return;
            if (Eldritch.Value < Info.Eldritch) return;

            await _gameActionFactory.Create(new RevealGameAction(this));
        }
    }
}
