﻿using DG.Tweening;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class CardShowerComponent : MonoBehaviour
    {
        private const float X_OFFSET = 6.5f;
        private CloneComponent _currentShowCard;
        private Tween _showcardSequence;

        /*******************************************************************/
        public void ShowCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;
            DestroyShowCard();
            transform.position = new Vector3(GetXPosition(cardView.transform), transform.position.y, transform.position.z);
            _currentShowCard = cardView.CloneToCardShower(transform);
            _showcardSequence = _currentShowCard.Animation();
        }

        public void HideCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;
            DestroyShowCard();
        }

        private void DestroyShowCard()
        {
            if (_currentShowCard == null) return;
            _showcardSequence?.Kill();
            Destroy(_currentShowCard.gameObject);
            _currentShowCard = null;
        }

        private float GetXPosition(Transform cardTransform)
        {
            Vector3 finalPosition = ((cardTransform.position - Camera.main.transform.position).normalized
               * (transform.position - Camera.main.transform.position).magnitude
               + Vector3.right * (Camera.main.WorldToViewportPoint(cardTransform.position).x < 0.5f ? X_OFFSET : -X_OFFSET));

            float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, transform.localPosition.z)).x + X_OFFSET * 2f;
            float rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, transform.localPosition.z)).x - X_OFFSET * 2f;
            return Mathf.Clamp(finalPosition.x, leftLimit, rightLimit);
        }

        private bool MustNotShowFilter(CardView cardView)
        {
            if (cardView.Card.FaceDown.IsActive) return true;
            if (cardView.CurrentZoneView.AvoidCardShower) return true;
            return false;
        }
    }
}
