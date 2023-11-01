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

        public Tween Repositionate(CardView _selectedCardView)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);

            int SelectedCardPosition = _allInvisibleHolders.IndexOf(GetInvisibleHolder(_selectedCardView));
            int AmountOfCards = _allInvisibleHolders.Count(invisibleHolder => !invisibleHolder.IsFree);

            Sequence repositionSequence = DOTween.Sequence();
            for (int i = 0; i < AmountOfCards; i++)
            {
                repositionSequence.Join(_allInvisibleHolders[i].Repositionate(Ypositionate(i)));
            }
            return repositionSequence;

            float Ypositionate(int i)
            {
                if (i <= SelectedCardPosition) return (AmountOfCards + i) * yOffSet;
                else return (AmountOfCards + SelectedCardPosition - i - SelectedCardPosition) * yOffSet;
            }
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
