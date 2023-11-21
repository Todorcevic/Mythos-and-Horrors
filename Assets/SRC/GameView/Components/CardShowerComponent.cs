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
            transform.position = new Vector3(GetPosition(cardView.transform).x, transform.position.y, transform.position.z);
            _currentShowCard = Instantiate(cardView, transform);
            _currentShowCard.DisableToShow();
            _currentShowCard.transform.localScale = Vector3.zero;
            _currentShowCard.transform.DOFullMove(transform).SetEase(Ease.InOutExpo).SetId(_currentShowCard.transform);
        }

        public void HideCard()
        {
            DOTween.Kill(_currentShowCard.transform);
            Destroy(_currentShowCard.gameObject);
        }

        private Vector3 GetPosition(Transform cardView) =>
            (Camera.main.transform.position + (cardView.position - Camera.main.transform.position).normalized
            * DISTANCE_FACTOR
            + Camera.main.transform.right *
            (Camera.main.WorldToViewportPoint(cardView.position).x < 0.5f ? X_OFFSET : -X_OFFSET));


    }
}
