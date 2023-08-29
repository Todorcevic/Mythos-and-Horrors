using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class SelectionCardGameAction : GameAction
    {
        [Inject] private readonly ICardMover _cardMovePresenter;
        [Inject] private readonly ICardActivator _cardActivatorPresenter;
        [Inject] private readonly GameActionFactory _gameActionRepository;
        private Card[] _cards;
        private Card cardSelected;

        /*******************************************************************/
        public async Task<Card> Run(params Card[] cards)
        {
            _cards = cards;
            await Start();
            return cardSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _cardMovePresenter.MoveCardsInFront(_cards.Select(card => card.Id).ToArray());
            _cardActivatorPresenter.ActivateThisCards(_cards.Select(card => card.Id).ToArray());

            cardSelected = await _gameActionRepository.Create<WaitingForSelectionGameAction>().Run();
        }
    }
}
