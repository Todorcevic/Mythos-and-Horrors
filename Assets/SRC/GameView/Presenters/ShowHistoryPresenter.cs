using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowHistoryPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        /*******************************************************************/
        public async Task PlayAnimationWith(ShowHistoryGameAction showHistoryGameAction)
        {
            Transform initialPosition = showHistoryGameAction.Card == null ? null : _cardViewsManager.GetCardView(showHistoryGameAction.Card).transform;
            await _showHistoryComponent.Show(showHistoryGameAction.History, initialPosition);
        }
    }
}
