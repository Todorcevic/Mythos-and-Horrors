using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayGameAction : GameAction
    {
        [Inject] private readonly IUAActivator _uIActivator;
        [Inject] private readonly CardsProvider _cardsProvider;
        private readonly TaskCompletionSource<bool> _waitForSelection = new();

        /*******************************************************************/
        public async Task Run()
        {
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card[] playabledCards = _cardsProvider.PlayabledCards();
            _uIActivator.HardActivate(playabledCards);
            await _waitForSelection.Task;
        }
    }
}

