using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class InvisibleHolder : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private LayoutElement layout;
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

        public bool HasThisCardView(CardView cardView) => _cardView == cardView;

        public Tween Repositionate(float yOffSet, float timeAnimation)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yOffSet, transform.localPosition.z);
            return _cardView.transform.DOFullLocalMove(transform, timeAnimation);
        }

        public void SetLayoutWidth(float width) => layout.preferredWidth = width;
    }
}
