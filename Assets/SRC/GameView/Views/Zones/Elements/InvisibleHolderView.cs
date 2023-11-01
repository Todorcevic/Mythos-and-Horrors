using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
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

        /*******************************************************************/
        public Tween AddCardView(CardView cardView)
        {
            _allCardView.Add(cardView, GetFreeHolder());
            _selectedCardView = cardView;
            return Repositionate();
        }

        public Tween RemoveCardView(CardView cardView)
        {
            _allCardView[cardView].gameObject.SetActive(false);
            _allCardView.Remove(cardView);
            return Repositionate();
        }

        public Tween RepositionateWith(CardView cardView)
        {
            _selectedCardView = cardView;
            return Repositionate();
        }

        public Tween Repositionate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);
            Sequence repositionSequence = DOTween.Sequence();
            List<CardView> entries = _allCardView.Keys.ToList();
            int selectedCardPosition = entries.IndexOf(_selectedCardView);

            for (int i = 0; i < entries.Count; i++)
            {
                Vector3 targetPosition = _allCardView[entries[i]].position + Ypositionate(i);
                repositionSequence.Join(entries[i].transform.DOMove(targetPosition, ViewValues.FAST_TIME_ANIMATION));
            }

            return repositionSequence;

            Vector3 Ypositionate(int i)
            {
                int amountOfCards = entries.Count;

                if (i <= selectedCardPosition) return new Vector3(0, (amountOfCards + i) * yOffSet, 0);
                else return new Vector3(0, (amountOfCards + selectedCardPosition - i - selectedCardPosition) * yOffSet, 0);
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
