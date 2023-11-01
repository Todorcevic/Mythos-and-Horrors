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
        float yOffSet = ViewValues.CARD_THICKNESS * 0.1f;

        [SerializeField, Required, ChildGameObjectsOnly] private List<Transform> _holders;
        [SerializeField, Required, ChildGameObjectsOnly] private RectTransform _invisibleHolderRect;
        private readonly Dictionary<CardView, Transform> _allCardView = new();
        private CardView _selectedCardView;
        private Sequence repositionSequence;

        /*******************************************************************/
        public Tween AddCardView(CardView cardView)
        {
            _allCardView.Add(cardView, GetFreeHolder());
            return RepositionateWithThisCard(cardView);
        }

        public Tween RemoveCardView(CardView cardView)
        {
            _allCardView[cardView].gameObject.SetActive(false);
            _allCardView.Remove(cardView);
            return Repositionate();
        }

        public Tween RepositionateWithThisCard(CardView cardView)
        {
            _selectedCardView = cardView;
            _allCardView[_selectedCardView].GetComponent<LayoutElement>().preferredWidth = 48;
            return Repositionate();
        }

        public Tween RepositionateExiting()
        {
            _allCardView[_selectedCardView].GetComponent<LayoutElement>().preferredWidth = 24;
            return Repositionate(0.15f);
        }

        private Tween Repositionate(float animationTime = ViewValues.FAST_TIME_ANIMATION)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);
            repositionSequence?.Kill();
            repositionSequence = DOTween.Sequence();
            List<CardView> entries = _allCardView.Keys.ToList();
            int selectedCardPosition = entries.IndexOf(_selectedCardView);
            int amountOfCards = entries.Count;

            for (int i = 0; i < entries.Count; i++)
            {
                Vector3 targetPosition = _allCardView[entries[i]].position;
                repositionSequence.Join(entries[i].transform.DOMoveX(targetPosition.x, animationTime))
                    .Join(entries[i].transform.DOMoveY(targetPosition.y + Ypositionate(i), animationTime))
                    .Join(entries[i].transform.DOMoveZ(targetPosition.z, animationTime));
            }

            return repositionSequence;

            float Ypositionate(int i)
            {
                if (i <= selectedCardPosition) return (amountOfCards + i) * yOffSet;
                else return (amountOfCards + selectedCardPosition - i - selectedCardPosition) * yOffSet;
            }
        }

        private Transform GetFreeHolder()
        {
            Transform holder = _holders.FirstOrDefault(holder => !holder.gameObject.activeSelf) ?? CreateNewHolder();
            holder.gameObject.SetActive(true);
            return holder;
        }

        private Transform CreateNewHolder()
        {
            Transform holder = Instantiate(_holders.First(), transform);
            _holders.Add(holder);
            return holder;
        }
    }
}
