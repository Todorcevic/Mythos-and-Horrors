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
        public IEnumerator AddCardView(CardView cardView)
        {
            _allCardView.Add(cardView, GetFreeHolder());
            return Reposicionate();
        }

        public IEnumerator RemoveCardView(CardView cardView)
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

        private IEnumerator Reposicionate()
        {
            yield return null;
            Sequence repositionSequence = DOTween.Sequence();
            _allCardView.ForEach(cardViewHolders => repositionSequence
                .Join(cardViewHolders.Key.transform.DOMove(cardViewHolders.Value.position, ViewValues.FAST_TIME_ANIMATION)));

        }

        private Transform CreateNewHolder()
        {
            Transform holder = Instantiate(_holders.First(), transform);
            _holders.Add(holder);
            return holder;
        }
    }
}
