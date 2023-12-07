using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveCardsGameAction : GameAction
    {
        private Card[] _cards;
        private Zone _zone;
        [Inject] private readonly ICardMover _cardMover;
        [Inject] private readonly IAdventurerSelector _adventurerSelector;

        /*******************************************************************/
        public async Task Run(Card[] cards, Zone zone)
        {
            _cards = cards;
            _zone = zone;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Card card in _cards)
            {
                card.CurrentZone?.RemoveCard(card);
                card.MoveToZone(_zone);
                _zone.AddCard(card);
            }

            await _adventurerSelector.Select(_zone);
            await _cardMover.MoveCardsToZoneAsync(_cards, _zone);
        }
    }
}

