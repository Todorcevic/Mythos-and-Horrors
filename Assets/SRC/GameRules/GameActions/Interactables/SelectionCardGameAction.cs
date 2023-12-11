using Sirenix.Utilities;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class SelectionCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMovePresenter;
        [Inject] private readonly IUAActivator _iUActivator;
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly ZonesProvider _zoneProvider;
        private Card[] _cards;
        private Card _cardSelected;

        /*******************************************************************/
        public async Task<Card> Run(params Card[] cards)
        {
            _cards = cards;
            await Start();
            return _cardSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cards.ForEach(card => _cardMovePresenter.MoveCardToZoneAsync(card, _zoneProvider.SelectorZone)); //TODO: make with MoveCardsToZoneAsync
            _iUActivator.HardActivate(_cards);
            _cardSelected = await _gameActionRepository.Create<WaitingForSelectionGameAction>().Run();
        }
    }
}
