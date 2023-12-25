using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlot : Card, IEndReactionable
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProviders;


        public int EldritchTotal { get; set; }

        /*******************************************************************/

        public async virtual Task WhenFinish(GameAction gameAction)
        {
            if (CanShowInitialHistory(gameAction))
                await _gameActionRepository.Create<ShowHistoryGameAction>().Run(Info.Histories[0]);

            if (CanShowFinalHistory(gameAction))
                await _gameActionRepository.Create<ShowHistoryGameAction>().Run(Info.Histories[1]);

        }

        protected virtual bool CanShowInitialHistory(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.Zone != _chaptersProviders.CurrentScene.PlotZone) return false;

            return true;
        }

        protected virtual bool CanShowFinalHistory(GameAction gameAction)
        {
            if (gameAction is not AddEldritchGameAction addEldritchGameAction) return false;
            if (EldritchTotal < Info.Eldritch) return false;

            return true;
        }
    }
}
