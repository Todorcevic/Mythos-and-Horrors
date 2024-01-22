using DG.Tweening;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardShowerComponent : MonoBehaviour
    {
        private const float X_OFFSET = 6.5f;
        private CardView _currentShowCard;

        /*******************************************************************/
        public void ShowCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;

            transform.position = new Vector3(GetPosition(cardView.transform).x, transform.position.y, transform.position.z);
            _currentShowCard = Instantiate(cardView, transform);
            _currentShowCard.DisableToShow();
            _currentShowCard.transform.ResetToZero();
            _currentShowCard.transform.DOFullMove(transform).SetEase(Ease.InOutExpo).SetId(_currentShowCard.transform);
        }

        public void HideCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;

            DOTween.Kill(_currentShowCard.transform);
            foreach (Transform child in gameObject.transform) Destroy(child.gameObject);
        }

        private Vector3 GetPosition(Transform cardTransform) =>
            (cardTransform.position - Camera.main.transform.position).normalized
            * (transform.position - Camera.main.transform.position).magnitude
            + Vector3.right * (Camera.main.WorldToViewportPoint(cardTransform.position).x < 0.5f ? X_OFFSET : -X_OFFSET);

        private bool MustNotShowFilter(CardView cardView)
        {
            if (cardView.Card.IsFaceDown) return true;
            if (cardView.CurrentZoneView.AvoidCardShower) return true;
            return false;
        }
    }
}
