﻿using DG.Tweening;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChangePositionPresenter : IPresenter<ChangeCardPositionGameAction>
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;

        /*******************************************************************/
        async Task IPresenter<ChangeCardPositionGameAction>.PlayAnimationWith(ChangeCardPositionGameAction changeCardPositionGameAction)
        {
            await UpdatePosition(changeCardPositionGameAction.Card);
        }

        /*******************************************************************/
        private async Task UpdatePosition(Card card)
        {
            Zone zone = card.CurrentZone;

            await _swapInvestigatorPresenter.Select(zone).AsyncWaitForCompletion();
            await _zoneViewsManager.Get(zone).UpdatePosition(card).AsyncWaitForCompletion();
        }
    }
}