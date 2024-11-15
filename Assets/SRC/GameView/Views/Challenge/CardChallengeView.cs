﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardChallengeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private CardView _cardView;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _cardImage;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeStatsController _challengeStatsControler;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        public Card Card { get; private set; }

        /*******************************************************************/
        public Tween SetCard(Card card, ChallengeType challengeType, int value)
        {
            if (Card != null) return DOTween.Sequence();
            _ = _cardImage.LoadCardSprite(card.Info.Code);
            Card = card;
            _cardView = _cardViewsManager.GetCardView(card);
            _challengeStatsControler.SetStat(challengeType, value);

            transform.localScale = Vector3.zero;
            return transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack)
                .OnStart(() => gameObject.SetActive(true));
        }

        public void Disable()
        {
            if (Card == null) return;
            Card = null;
            gameObject.SetActive(false);
        }

        /*******************************************************************/
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _cardImage.DOColor(new Color(0.8f, 0.8f, 0.8f), ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            _cardView.CardSensor.MouseEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _cardImage.DOColor(Color.white, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
            _cardView.CardSensor.MouseExit();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _cardView.CardSensor.MouseUpAsButton();
        }
    }
}