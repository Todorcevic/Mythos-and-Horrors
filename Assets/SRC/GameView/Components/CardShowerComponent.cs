using DG.Tweening;
using UnityEngine;

namespace MythsAndHorrors.GameView
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

            transform.position = new Vector3(GetPosition(cardView.transform).x, transform.position.y, transform.position.z);
            _currentShowCard = cardView.Clone(transform);
            _showcardSequence = _currentShowCard.Animation();
        }

        public void HideCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;

            _showcardSequence?.Kill();
            Destroy(_currentShowCard.gameObject);
        }

        private Vector3 GetPosition(Transform cardTransform) =>
            (cardTransform.position - Camera.main.transform.position).normalized
            * (transform.position - Camera.main.transform.position).magnitude
            + Vector3.right * (Camera.main.WorldToViewportPoint(cardTransform.position).x < 0.5f ? X_OFFSET : -X_OFFSET);

        private bool MustNotShowFilter(CardView cardView)
        {
            if (cardView.Card.FaceDown.IsActive) return true;
            if (cardView.CurrentZoneView.AvoidCardShower) return true;
            return false;
        }
    }
}
