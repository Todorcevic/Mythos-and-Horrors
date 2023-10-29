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

        /*******************************************************************/
        public Tween AddCardView(CardView cardView)
        {
            _allCardView.Add(cardView, GetFreeHolder());
            return Reposicionate();
        }

        public Tween RemoveCardView(CardView cardView)
        {
            _allCardView[cardView].gameObject.SetActive(false);
            _allCardView.Remove(cardView);
            return Reposicionate();
        }

        private Transform GetFreeHolder()
        {
            Transform holder = _holders.FirstOrDefault(holder => !holder.gameObject.activeSelf) ?? CreateNewHolder();
            holder.gameObject.SetActive(true);
            return holder;
        }

        private Tween Reposicionate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_invisibleHolderRect);
            Sequence repositionSequence = DOTween.Sequence();
            List<KeyValuePair<CardView, Transform>> entries = _allCardView.ToList();
            for (int i = 0; i < entries.Count; i++)
            {
                Vector3 targetPosition = entries[i].Value.position + new Vector3(0, i * yOffSet, 0);
                repositionSequence.Join(entries[i].Key.transform.DOMove(targetPosition, ViewValues.FAST_TIME_ANIMATION));
            }
            return repositionSequence;
        }

        private Transform CreateNewHolder()
        {
            Transform holder = Instantiate(_holders.First(), transform);
            _holders.Add(holder);
            return holder;
        }

        public void ShowCard(CardView cardView)
        {
            Transform invisibleHolder = _allCardView[cardView];
            cardView.transform.DOLocalMoveZ(invisibleHolder.localPosition.z + 6, ViewValues.FAST_TIME_ANIMATION);
            //cardView.transform.DOScale(1.2f, ViewValues.FAST_TIME_ANIMATION);
            //invisibleHolder.GetComponent<LayoutElement>().preferredWidth = 30;
            //RemoveCardView(cardView);
        }

        public void HideCard(CardView cardView)
        {
            Transform invisibleHolder = _allCardView[cardView];
            cardView.transform.DOLocalMoveZ(invisibleHolder.localPosition.z, ViewValues.FAST_TIME_ANIMATION);
            //cardView.transform.DOScale(1, ViewValues.FAST_TIME_ANIMATION);
            //invisibleHolder.GetComponent<LayoutElement>().preferredWidth = 24;
            //AddCardView(cardView);
        }
    }
}
