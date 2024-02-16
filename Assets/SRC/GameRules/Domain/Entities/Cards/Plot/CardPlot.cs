using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlot : Card, IEndReactionable, IRevellable
    {
        private bool _isRevealed;
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly List<History> _histories;

        public Stat Eldritch { get; private set; }
        public History InitialHistory => _histories[0];
        public History FinalHistory => _histories[1];


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
            if (CanShowInitialHistory(gameAction)) await _gameActionFactory.Create(new RevealGameAction(this)); //TODO ShowHistoryGameAction wellcome
            if (CanShowFinalHistory(gameAction)) await _gameActionFactory.Create(new RevealGameAction(this));
        }

        void IRevellable.Reveal() => _isRevealed = true;

        protected virtual bool CanShowInitialHistory(GameAction gameAction)
        {
            if (_isRevealed) return false;
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.PlotZone) return false;
            if (_histories == null || _histories.Count < 1) return false;

            return true;
        }

        protected virtual bool CanShowFinalHistory(GameAction gameAction)
        {
            if (_isRevealed) return false;
            if (gameAction is not StatGameAction statGameAction) return false;
            if (statGameAction.Stat != Eldritch) return false;
            if (Eldritch.Value < Info.Eldritch) return false;
            if (_histories == null || _histories.Count < 2) return false;

            return true;
        }


    }
}
