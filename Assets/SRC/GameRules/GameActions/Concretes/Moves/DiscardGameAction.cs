using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public IEnumerable<Card> Cards { get; private set; }

        /*******************************************************************/
        public DiscardGameAction SetWith(Card card) => SetWith(new Card[] { card });

        public DiscardGameAction SetWith(IEnumerable<Card> cards)
        {
            Cards = cards;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Card, Zone> discardResult = Cards.ToDictionary(card => card, card => GetDiscardZone(card));
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(discardResult).Execute();
            await _gameActionsProvider.Create<ResetCardGameAction>().SetWith(Cards).Execute();
        }

        private Zone GetDiscardZone(Card card)
        {
            if (card.IsVictory) return _chaptersProvider.CurrentScene.VictoryZone;

            if (_chaptersProvider.CurrentScene.StartDeckDangerCards.Contains(card))
                return _chaptersProvider.CurrentScene.DangerDiscardZone;

            return card.Owner?.DiscardZone ?? _chaptersProvider.CurrentScene.OutZone;
        }
    }
}
