using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class SelectionCardGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private Card _cardSelected;

        /*******************************************************************/
        public async Task<Card> Run(params Card[] cards)
        {
            ActivableCards = cards.ToList();
            await Start();
            return _cardSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create<MoveCardsGameAction>().Run(ActivableCards, _chaptersProvider.CurrentScene.SelectorZone);
            _cardSelected = await _gameActionRepository.Create<WaitingForSelectionGameAction>().Run();
        }
    }
}
