using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class InvisibleHolderView : MonoBehaviour
    {
        readonly float yOffSet = ViewValues.CARD_THICKNESS * 0.1f;

        [SerializeField, Required, ChildGameObjectsOnly] private RectTransform _invisibleHolderRect;
        [SerializeField, Required, ChildGameObjectsOnly] private List<InvisibleHolder> _allInvisibleHolders = new();

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

        public Tween Repositionate(CardView _selectedCardView, float layout = 24)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);

            int SelectedCardPosition = _allInvisibleHolders.IndexOf(GetInvisibleHolder(_selectedCardView));
            int AmountOfCards = _allInvisibleHolders.Count(invisibleHolder => !invisibleHolder.IsFree);

            Sequence repositionSequence = DOTween.Sequence();
            for (int i = 0; i < AmountOfCards; i++)
            {
                if (i == SelectedCardPosition) _allInvisibleHolders[i].SetLayoutWidth(layout);
                float realYOffSet = (AmountOfCards + (i <= SelectedCardPosition ? i : -i)) * yOffSet;
                repositionSequence.Join(_allInvisibleHolders[i].Repositionate(realYOffSet));
            }
            return repositionSequence;
        }

        private InvisibleHolder GetFreeHolder()
        {
            InvisibleHolder holder = _allInvisibleHolders.FirstOrDefault(invisiblerHolder => invisiblerHolder.IsFree) ?? CreateNewHolder();
            holder.gameObject.SetActive(true);
            return holder;
        }

        private InvisibleHolder CreateNewHolder()
        {
            InvisibleHolder invisibleHolder = Instantiate(_allInvisibleHolders.First(), transform);
            _allInvisibleHolders.Add(invisibleHolder);
            return invisibleHolder;
        }

        public InvisibleHolder GetInvisibleHolder(CardView cardView)
        {
            return _allInvisibleHolders.Find(invisibleHolder => invisibleHolder.HasThisCardView(cardView));
        }
    }
}
