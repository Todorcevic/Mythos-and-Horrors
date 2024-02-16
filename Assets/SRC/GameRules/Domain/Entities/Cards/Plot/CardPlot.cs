using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlot : Card, IEndReactionable
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public Stat Eldritch { get; private set; }

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
            if (CanShowInitialHistory(gameAction))
                await _gameActionRepository.Create(new ShowHistoryGameAction(Histories[0]));

            if (CanShowFinalHistory(gameAction))
            {
                await _gameActionRepository.Create(new ShowHistoryGameAction(Histories[1]));
            }
        }

        protected virtual bool CanShowInitialHistory(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.PlotZone) return false;
            if (Histories == null || Histories.Count < 1) return false;

            return true;
        }

        protected virtual bool CanShowFinalHistory(GameAction gameAction)
        {
            if (gameAction is not AddEldritchGameAction addEldritchGameAction) return false;
            if (Eldritch.Value < Info.Eldritch) return false;
            if (Histories == null || Histories.Count < 2) return false;

            return true;
        }
    }
}
