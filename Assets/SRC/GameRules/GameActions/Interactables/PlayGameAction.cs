using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayGameAction : GameAction
    {
        [Inject] private readonly IUIActivator _uIActivator;
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
            List<Card> playabledCards = _cardsProvider.PlayabledCards();
            _uIActivator.Activate(playabledCards);
            await _waitForSelection.Task;
        }
    }
}

