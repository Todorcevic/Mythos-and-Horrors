using DG.Tweening;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardShowerComponent : MonoBehaviour
    {
        private const float X_OFFSET = 7f;
        private const float DISTANCE_FACTOR = 30f;
        private CardView _currentShowCard;

        /*******************************************************************/
        public void ShowCard(CardView cardView)
        {
            if (MustNotShow(cardView)) return;

            transform.position = new Vector3(GetPosition(cardView.transform).x, transform.position.y, transform.position.z);
            _currentShowCard = Instantiate(cardView, transform);
            _currentShowCard.DisableToShow();
            _currentShowCard.transform.ResetToZero();
            _currentShowCard.transform.DOFullMove(transform).SetEase(Ease.InOutExpo).SetId(_currentShowCard.transform);
        }

        public void HideCard(CardView cardView)
        {
            if (MustNotShow(cardView)) return;

            DOTween.Kill(_currentShowCard.transform);
            foreach (Transform child in gameObject.transform) Destroy(child.gameObject);
        }

        private Vector3 GetPosition(Transform cardView) =>
            (Camera.main.transform.position + (cardView.position - Camera.main.transform.position).normalized
            * DISTANCE_FACTOR
            + Camera.main.transform.right *
            (Camera.main.WorldToViewportPoint(cardView.position).x < 0.5f ? X_OFFSET : -X_OFFSET));

        private bool MustNotShow(CardView cardView) => cardView.Card.IsFaceDown || cardView.CurrentZoneView is ZoneHandView;
    }
}
