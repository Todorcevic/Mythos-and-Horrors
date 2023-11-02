using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class InvisibleHolder : MonoBehaviour
    {
        [SerializeField] private LayoutElement layout;
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
            //return DOTween.Sequence().Join(_cardView.transform.DOMoveX(transform.position.x, ViewValues.FAST_TIME_ANIMATION))
            //     .Join(_cardView.transform.DOMoveY(transform.position.y + yOffSet, ViewValues.FAST_TIME_ANIMATION))
            //     .Join(_cardView.transform.DOMoveZ(transform.position.z, ViewValues.FAST_TIME_ANIMATION));

            return _cardView.transform.DOMove(transform.position + new Vector3(0, yOffSet, 0), ViewValues.FAST_TIME_ANIMATION);
        }

        public void SetLayoutWidth(float width)
        {
            layout.preferredWidth = width;
        }
    }
}
