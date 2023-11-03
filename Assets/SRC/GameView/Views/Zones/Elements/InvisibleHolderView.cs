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
        private const float Y_OFF_SET = ViewValues.CARD_THICKNESS * 0.1f;
        [SerializeField, Required, ChildGameObjectsOnly] private RectTransform _invisibleHolderRect;
        [SerializeField, Required, ChildGameObjectsOnly] private List<InvisibleHolder> _allInvisibleHolders = new();

        List<InvisibleHolder> AllActivesInvisibleHolders => _allInvisibleHolders.FindAll(invisibleHolder => !invisibleHolder.IsFree);

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
            int SelectedCardPosition = _allInvisibleHolders.IndexOf(GetInvisibleHolder(_selectedCardView));
            int AmountOfCards = _allInvisibleHolders.Count(invisibleHolder => !invisibleHolder.IsFree);
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

        public InvisibleHolder GetInvisibleHolder(CardView cardView) =>
            _allInvisibleHolders.Find(invisibleHolder => invisibleHolder.HasThisCardView(cardView));

    }
}
