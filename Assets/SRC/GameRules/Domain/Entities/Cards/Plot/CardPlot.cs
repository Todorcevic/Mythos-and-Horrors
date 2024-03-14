using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardPlot : Card, IRevealable
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public Stat Eldritch { get; private set; }
        public State Revealed { get; private set; }
        public bool IsComplete => Eldritch.Value <= 0;
        public int Position => _chaptersProviders.CurrentScene.Info.PlotCards.IndexOf(this);
        public CardPlot NextCardPlot => _chaptersProviders.CurrentScene.Info.PlotCards.ElementAtOrDefault(Position + 1);

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
        }

        /*******************************************************************/
        public virtual async Task RevealEffect()
        {
            await _gameActionProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
            await _gameActionProvider.Create(new DiscardGameAction(this));
            await _gameActionProvider.Create(new PlacePlotGameAction(NextCardPlot));
        }
    }
}
