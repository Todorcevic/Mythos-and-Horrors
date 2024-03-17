using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowHistoryPresenter : IPresenter<ShowHistoryGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        /*******************************************************************/
        async Task IPresenter<ShowHistoryGameAction>.PlayAnimationWith(ShowHistoryGameAction showHistoryGameAction)
        {
            Transform initialPosition = showHistoryGameAction.Card == null ? null : _cardViewsManager.GetCardView(showHistoryGameAction.Card).transform;
            await _showHistoryComponent.Show(showHistoryGameAction.History, initialPosition);
        }

        /*******************************************************************/
        Task IPresenter<ShowHistoryGameAction>.UndoAnimationWith(ShowHistoryGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
