using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Card Card { get; private set; }

        /*******************************************************************/
        public DiscardGameAction SetWith(Card card)
        {
            Card = card;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Zone zoneToMove = GetDiscardZone();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Card, zoneToMove).Start();
            await _gameActionsProvider.Create(new ResetCardGameAction(Card));
        }

        private Zone GetDiscardZone()
        {
            if (Card.IsVictory) return _chaptersProvider.CurrentScene.VictoryZone;

            if (_chaptersProvider.CurrentScene.StartDeckDangerCards.Contains(Card))
                return _chaptersProvider.CurrentScene.DangerDiscardZone;

            return Card.Owner?.DiscardZone ?? _chaptersProvider.CurrentScene.OutZone;
        }
    }
}
