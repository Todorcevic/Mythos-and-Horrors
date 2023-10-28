using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
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
            for (int i = 0; i < _allCardView.Count; i++)
            {
                CardView cardView = entries[i].Key;
                Transform targetTransform = entries[i].Value;
                Vector3 targetPosition = targetTransform.position + new Vector3(0, i * yOffSet, 0);
                repositionSequence.Join(cardView.transform.DOMove(targetPosition, ViewValues.FAST_TIME_ANIMATION));
            }
            return repositionSequence;
        }

        private Transform CreateNewHolder()
        {
            Transform holder = Instantiate(_holders.First(), transform);
            _holders.Add(holder);
            return holder;
        }
    }
}
