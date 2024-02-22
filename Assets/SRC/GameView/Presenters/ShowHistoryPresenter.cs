using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : INewPresenter<ShowHistoryGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        /*******************************************************************/
        async Task INewPresenter<ShowHistoryGameAction>.PlayAnimationWith(ShowHistoryGameAction showHistoryGameAction)
        {
            Transform initialPosition = showHistoryGameAction.Card == null ? null : _cardViewsManager.GetCardView(showHistoryGameAction.Card).transform;
            await _showHistoryComponent.Show(showHistoryGameAction.History, initialPosition);
        }
    }
}
