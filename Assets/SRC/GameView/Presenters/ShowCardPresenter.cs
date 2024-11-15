﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowCardPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(ShowCardsGameAction ShowCardsGameAction)
        {
            if (ShowCardsGameAction.Parent is DrawGameAction) await ShowDrawedCard(ShowCardsGameAction.Cards.Unique());
        }

        private async Task ShowDrawedCard(Card cardDrawed)
        {
            _mainButtonComponent.SetEffect(new BaseEffect(null, null, PlayActionType.None, null, new Localization("MainButton_Continue")));
            await DOTween.Sequence()
                 .Join(_moveCardHandler.MoveCardtoCenter(cardDrawed))
                 .Join(_showSelectorComponent.MainButtonWaitingToContinueShowUp()).AsyncWaitForCompletion();

            Tween idle = _cardViewsManager.GetCardView(cardDrawed).Idle();
            await _clickHandler.WaitingClick();
            idle.Kill();

            _showSelectorComponent.MainButtonWaitingToContinueHideUp();
        }
    }
}
