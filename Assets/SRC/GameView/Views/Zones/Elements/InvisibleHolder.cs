using DG.Tweening;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class InvisibleHolder : MonoBehaviour
    {
        private CardView _cardView;

        public bool IsFree => _cardView == null;

        /*******************************************************************/
        public void SetCardView(CardView cardView)
        {
            gameObject.SetActive(true);
            _cardView = cardView;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
            _cardView = null;
        }

        public bool HasThisCardView(CardView cardView)
        {
            return _cardView == cardView;
        }

        public Tween Repositionate(float yOffSet = 0)
        {
            return _cardView.transform.DOMove(transform.position + new Vector3(0, yOffSet, 0), ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
