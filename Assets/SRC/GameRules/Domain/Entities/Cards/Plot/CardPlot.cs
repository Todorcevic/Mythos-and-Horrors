using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlot : Card, IEndReactionable
    {
        private bool _isRevealed;
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly List<History> _histories;

        public Stat Eldritch { get; private set; }
        public History InitialHistory => _histories[0];
        public History RevealHistory => _histories[1];


        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Eldritch = new Stat(Info.Eldritch ?? 0, Info.Eldritch ?? 0);
        }

        /*******************************************************************/
        async Task IEndReactionable.WhenFinish(GameAction gameAction)
        {
            await CanShowInitialHistory(gameAction);
            await CanShowFinalHistory(gameAction);
        }

        protected virtual async Task CanShowInitialHistory(GameAction gameAction)
        {
            if (_isRevealed) return;
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return;
            if (!moveCardsGameAction.Cards.Contains(this)) return;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.PlotZone) return;
            if (_histories == null || _histories.Count < 1) return;

            await _gameActionFactory.Create(new ShowHistoryGameAction(InitialHistory));
        }

        protected virtual async Task CanShowFinalHistory(GameAction gameAction)
        {
            if (_isRevealed) return;
            if (gameAction is not StatGameAction statGameAction) return;
            if (statGameAction.Stat != Eldritch) return;
            if (Eldritch.Value < Info.Eldritch) return;
            if (_histories == null || _histories.Count < 2) return;

            await Reveal();
        }

        protected virtual async Task Reveal()
        {
            _isRevealed = true;
            await _gameActionFactory.Create(new RevealGameAction(this));
            await _gameActionFactory.Create(new ShowHistoryGameAction(RevealHistory));
        }
    }
}
