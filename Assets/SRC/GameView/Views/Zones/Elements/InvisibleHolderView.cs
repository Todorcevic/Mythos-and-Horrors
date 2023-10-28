using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class InvisibleHolderView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<Transform> _holders;
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
            Sequence repositionSequence = DOTween.Sequence().AppendInterval(ViewValues.FAST_TIME_ANIMATION);
            _allCardView.ForEach(cardViewHolders => repositionSequence
                .Join(cardViewHolders.Key.transform.DOMove(cardViewHolders.Value.position, ViewValues.FAST_TIME_ANIMATION)));
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
