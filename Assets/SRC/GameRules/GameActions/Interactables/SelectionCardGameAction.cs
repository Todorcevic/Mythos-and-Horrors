using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class SelectionCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMovePresenter;
        [Inject] private readonly IUIActivator _iUActivator;
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private List<Card> _cards;
        private Card _cardSelected;

        /*******************************************************************/
        public async Task<Card> Run(params Card[] cards)
        {
            _cards = cards.ToList();
            await Start();
            return _cardSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cards.ForEach(card => _cardMovePresenter.MoveCardToZone(card, _chaptersProvider.CurrentScene.SelectorZone)); //TODO: make with MoveCardsToZoneAsync
            _iUActivator.Activate(_cards);
            _cardSelected = await _gameActionRepository.Create<WaitingForSelectionGameAction>().Run();
        }
    }
}
