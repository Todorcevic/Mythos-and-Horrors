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
        public bool IsComplete => Eldritch.Value <= 0;
        public CardPlot NextCardPlot => _chaptersProviders.CurrentScene.Info.PlotCards.NextElementFor(this);

        /*******************************************************************/
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Eldritch = new Stat(Info.Eldritch ?? 0);
            Revealed = new State(false);
        }

        /*******************************************************************/
        public async Task RevealEffect()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
            await CompleteEffect();
            await _gameActionsProvider.Create(new DiscardGameAction(this));
            await _gameActionsProvider.Create(new PlacePlotGameAction(NextCardPlot));
        }

        public abstract Task CompleteEffect();
    }
}
