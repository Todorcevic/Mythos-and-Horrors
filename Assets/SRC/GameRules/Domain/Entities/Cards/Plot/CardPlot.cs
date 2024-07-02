using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardPlot : Card, IRevealable
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Eldritch { get; private set; }
        public State Revealed { get; private set; }
        public GameCommand<RevealGameAction> RevealCommand { get; private set; }

        /*******************************************************************/
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);
        public CardPlot NextCardPlot => _chaptersProviders.CurrentScene.PlotCards.NextElementFor(this);
        public int AmountOfEldritch => (Info.Eldritch ?? 0) - Eldritch.Value;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Eldritch = CreateStat(Info.Eldritch ?? 0);
            Revealed = CreateState(false);
            RevealCommand = new GameCommand<RevealGameAction>(RevealEffect);
        }

        /*******************************************************************/
        protected virtual async Task RevealEffect(RevealGameAction revealGameAction)
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(RevealHistory, this).Execute();
            await CompleteEffect();
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<PlacePlotGameAction>().SetWith(NextCardPlot).Execute();
        }

        protected abstract Task CompleteEffect();
    }
}
