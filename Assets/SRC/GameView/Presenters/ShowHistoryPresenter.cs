using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ShowHistoryComponent _showHistoryComponent;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is ShowHistoryGameAction showHistoryGameAction)
            {
                Transform initialPosition = showHistoryGameAction.Card == null ? null : _cardViewsManager.GetCardView(showHistoryGameAction.Card).transform;
                await _showHistoryComponent.Show(showHistoryGameAction.History, initialPosition);
            }
        }
    }
}
