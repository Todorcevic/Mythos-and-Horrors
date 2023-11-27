using Sirenix.Utilities;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class SelectionCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMovePresenter;
        [Inject] private readonly ICardActivator _cardActivatorPresenter;
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
            _cards.ForEach(card => _cardMovePresenter.MoveCardToZone(card, _zoneProvider.SelectorZone));
            _cardActivatorPresenter.ActivateThisCards(_cards);
            _cardSelected = await _gameActionRepository.Create<WaitingForSelectionGameAction>().Run();
        }
    }
}
