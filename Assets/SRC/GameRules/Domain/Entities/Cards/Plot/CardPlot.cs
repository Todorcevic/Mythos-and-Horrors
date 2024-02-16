using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlot : Card, IEndReactionable
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly List<History> _histories;
        [Inject] private readonly IShowHistory _showHistory;

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
        public async virtual Task WhenFinish(GameAction gameAction)
        {
            if (CanShowInitialHistory(gameAction)) await _showHistory.Show(InitialHistory);
            if (CanShowFinalHistory(gameAction)) await _showHistory.Show(FinalHistory);
        }

        protected virtual bool CanShowInitialHistory(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.PlotZone) return false;
            if (_histories == null || _histories.Count < 1) return false;

            return true;
        }

        protected virtual bool CanShowFinalHistory(GameAction gameAction)
        {
            if (gameAction is not AddEldritchGameAction addEldritchGameAction) return false;
            if (Eldritch.Value < Info.Eldritch) return false;
            if (_histories == null || _histories.Count < 2) return false;

            return true;
        }
    }
}
