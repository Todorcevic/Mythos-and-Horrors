﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SwapInvestigatorHandler
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;
        private Investigator _investigatorSelected;

        /*******************************************************************/
        public Tween Select(Investigator investigator)
        {
            if (investigator == null || _investigatorSelected == investigator) return DOTween.Sequence();
            Sequence swapSequence = DOTween.Sequence()
            .Append(_swapInvestigatorComponent.Select(investigator))
            .Join(_avatarViewsManager.Get(_investigatorSelected ?? investigator).Deselect())
            .Join(_avatarViewsManager.Get(investigator).Select());
            _investigatorSelected = investigator;
            return swapSequence;
        }

        public Tween Select(Zone zone)
        {
            Investigator investigator = zone?.Owner ?? _investigatorSelected;
            return Select(investigator);
        }
    }
}