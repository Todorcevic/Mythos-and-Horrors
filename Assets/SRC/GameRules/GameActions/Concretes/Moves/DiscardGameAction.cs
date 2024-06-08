using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Card Card { get; }

        /*******************************************************************/
        public DiscardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Zone zoneToMove = GetDiscardZone();
            await _gameActionsProvider.Create(new MoveCardsGameAction(Card, zoneToMove));
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
