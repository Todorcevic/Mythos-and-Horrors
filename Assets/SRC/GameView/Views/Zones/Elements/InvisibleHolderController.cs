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
        [SerializeField, Required, ChildGameObjectsOnly] private RectTransform _invisibleHolderRect;
        [SerializeField, Required, ChildGameObjectsOnly] private List<InvisibleHolder> _allInvisibleHolders;

        public List<InvisibleHolder> AllActivesInvisibleHolders => _allInvisibleHolders.FindAll(invisibleHolder => !invisibleHolder.IsFree);
        public int AmountOfCards => AllActivesInvisibleHolders.Count();

        /*******************************************************************/
        public Tween AddCardView(CardView cardView)
        {
            GetFreeHolder().SetCardView(cardView);
            return Repositionate(cardView);
        }

        public Tween RemoveCardView(CardView cardView) //Check if launch exception
        {
            GetInvisibleHolder(cardView).Clear();
            return Repositionate(cardView);
        }

        public Transform SetLayout(CardView cardView, float layoutAmount)
        {
            InvisibleHolder invisibleHolder = GetInvisibleHolder(cardView);
            if (AmountOfCards > 3) invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH * layoutAmount);
            Repositionate(cardView);
            return invisibleHolder.transform;
        }

        public Tween ResetLayout(CardView cardView)
        {
            InvisibleHolder invisibleHolder = GetInvisibleHolder(cardView);
            invisibleHolder.SetLayoutWidth(ViewValues.INITIAL_LAYOUT_WIDTH);
            return Repositionate(cardView);
        }

        private Tween Repositionate(CardView cardView)
        {
            int SelectedCardPosition = _allInvisibleHolders.IndexOf(GetInvisibleHolder(cardView));
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);
            Sequence repositionSequence = DOTween.Sequence();
            for (int i = 0; i < AmountOfCards; i++)
            {
                float realYOffSet = (AmountOfCards + (i <= SelectedCardPosition ? i : -i)) * Y_OFF_SET;
                repositionSequence.Join(AllActivesInvisibleHolders[i].Repositionate(realYOffSet));
            }
            return repositionSequence;
        }

        private InvisibleHolder GetFreeHolder() =>
            _allInvisibleHolders.FirstOrDefault(invisiblerHolder => invisiblerHolder.IsFree) ?? CreateNewHolder();


        private InvisibleHolder CreateNewHolder()
        {
            InvisibleHolder invisibleHolder = Instantiate(_allInvisibleHolders.First(), transform);
            _allInvisibleHolders.Add(invisibleHolder);
            return invisibleHolder;
        }

        private InvisibleHolder GetInvisibleHolder(CardView cardView) =>
            _allInvisibleHolders.Find(invisibleHolder => invisibleHolder.HasThisCardView(cardView));
    }
}
