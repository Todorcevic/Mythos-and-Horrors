﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatUpdatePresenter : IPresenter<StatGameAction>
    {
        [Inject] private readonly StatableManager _statsViewsManager;

        /*******************************************************************/
        async Task IPresenter<StatGameAction>.PlayAnimationWith(StatGameAction updateStatGameAction)
        {
            UpdateStat(updateStatGameAction);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        private Sequence UpdateStat(StatGameAction updateStatGameAction)
        {
            List<IStatableView> statables = _statsViewsManager.GetAll(updateStatGameAction.Stat);
            Sequence updateStatsSequence = DOTween.Sequence();
            statables.ForEach(statView => updateStatsSequence.Join(statView.UpdateValue()));
            return updateStatsSequence;
        }
    }
}