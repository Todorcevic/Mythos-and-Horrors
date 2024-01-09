using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayGameAction : InteractableGameAction
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        private readonly TaskCompletionSource<bool> _waitForSelection = new();

        /*******************************************************************/
        public async Task Run()
        {
            ActivableCards = _cardsProvider.PlayabledCards();
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _waitForSelection.Task;
        }
    }
}

