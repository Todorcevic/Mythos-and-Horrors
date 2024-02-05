using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardShowerComponent : MonoBehaviour
    {
        private const float X_OFFSET = 6.5f;
        private CloneComponent _currentShowCard;

        /*******************************************************************/
        public void ShowCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;

            transform.position = new Vector3(GetPosition(cardView.transform).x, transform.position.y, transform.position.z);
            _currentShowCard = cardView.Clone(transform);
            _currentShowCard.ShowEffects(cardView.Card.PlayableEffects.ToArray());
            _currentShowCard.DisableGlow();
            _currentShowCard.transform.ResetToZero();

            DOTween.Sequence()
                .Join(_currentShowCard.transform.DORecolocate().SetEase(Ease.InOutExpo))
                .SetId(_currentShowCard);
        }

        public void HideCard(CardView cardView)
        {
            if (MustNotShowFilter(cardView)) return;
            DOTween.Kill(_currentShowCard);
            Destroy(_currentShowCard.gameObject);
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
