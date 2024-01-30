using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class InvisibleHolderController : MonoBehaviour
    {
        private const float Y_OFF_SET = ViewValues.CARD_THICKNESS * 0.1f;
        private Sequence repositionSequence;
        [SerializeField, Required, ChildGameObjectsOnly] private RectTransform _invisibleHolderRect;
        [SerializeField, Required, ChildGameObjectsOnly] private List<InvisibleHolder> _allInvisibleHolders;

        public List<InvisibleHolder> AllActivesInvisibleHolders => _allInvisibleHolders.Where(invisibleHolder => !invisibleHolder.IsFree)
            .OrderBy(invisibleHolder => invisibleHolder.transform.GetSiblingIndex()).ToList();

        public int AmountOfCards => AllActivesInvisibleHolders.Count();

        /*******************************************************************/
        public Tween AddCardView(CardView cardView)
        {
            GetFreeHolder().SetCardView(cardView);
            int selectedCardPosition = GetInvisibleHolderIndex(cardView);
            return Repositionate(selectedCardPosition, withFast: false);
        }

        public Tween RemoveCardView(CardView cardView)
        {
            int selectedCardPosition = GetInvisibleHolderIndex(cardView);
            GetInvisibleHolder(cardView).Clear();
            return Repositionate(selectedCardPosition, withFast: false);

        }

        public Transform SetLayout(CardView cardView, float layoutAmount)
        {
            repositionSequence?.Kill();
            InvisibleHolder invisibleHolder = GetInvisibleHolder(cardView);
            if (AmountOfCards > 3) invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH * layoutAmount);
            Repositionate(GetInvisibleHolderIndex(cardView), withFast: true);
            return invisibleHolder.transform;
        }

        public Tween ResetLayout(CardView cardView)
        {
            repositionSequence?.Kill();
            InvisibleHolder invisibleHolder = GetInvisibleHolder(cardView);
            invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH);
            return Repositionate(GetInvisibleHolderIndex(cardView), withFast: true);
        }

        private Tween Repositionate(int selectedCardPosition, bool withFast)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);
            repositionSequence = DOTween.Sequence();

            for (int i = 0; i < AmountOfCards; i++)
            {
                float realYOffSet = (AmountOfCards + (i <= selectedCardPosition ? i : -i)) * Y_OFF_SET;
                float animationTime = (i == selectedCardPosition && withFast) ? ViewValues.FAST_TIME_ANIMATION : ViewValues.DEFAULT_TIME_ANIMATION;
                repositionSequence.Join(AllActivesInvisibleHolders[i].Repositionate(realYOffSet, animationTime));
            }

            return repositionSequence;
        }

        private InvisibleHolder GetFreeHolder() =>
            _allInvisibleHolders.FirstOrDefault(invisiblerHolder => invisiblerHolder.IsFree) ?? CreateNewHolder();

        private InvisibleHolder CreateNewHolder()
        {
            InvisibleHolder invisibleHolder = Instantiate(_allInvisibleHolders.First(), transform);
            _allInvisibleHolders.Add(invisibleHolder);
            invisibleHolder.name = $"Holder{_allInvisibleHolders.Count}";
            return invisibleHolder;
        }

        private InvisibleHolder GetInvisibleHolder(CardView cardView) =>
            _allInvisibleHolders.First(invisibleHolder => invisibleHolder.HasThisCardView(cardView));

        private int GetInvisibleHolderIndex(CardView cardView) =>
            AllActivesInvisibleHolders.IndexOf(GetInvisibleHolder(cardView));
    }
}
