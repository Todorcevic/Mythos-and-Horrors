using System.Collections.Generic;
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
        [Inject] private readonly CardsProvider _cardsProvider;

        public Stat Eldritch { get; private set; }
        public State Revealed { get; private set; }
        public GameCommand<RevealGameAction> RevealCommand { get; private set; }
        public override IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();

        /*******************************************************************/
        public History InitialHistory => Info.Histories?.ElementAt(0) ?? new History(); //TODO: Remove control when all cards have history
        public History RevealHistory => Info.Histories?.ElementAt(1) ?? new History(); //TODO: Remove control when all cards have history
        public CardPlot NextCardPlot => _chaptersProviders.CurrentScene.PlotCards.NextElementFor(this);
        public int AmountOfEldritch => Eldritch.Value - _cardsProvider.AllCards.OfType<IEldritchable>().Sum(eldrichable => eldrichable.Eldritch.Value);
        public int AmountDecrementedEldritch => (Info.Eldritch ?? 0) - AmountOfEldritch;

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
