using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class EldritchStatView : MonoBehaviour, IStatable
    {
        [Inject] private readonly StatableManager _statableManager;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;

        public Stat Stat { get; private set; }
        public List<Stat> MultiStat { get; private set; }
        public Transform StatTransform => transform;
        public int TotalValue => Stat.Value - MultiStat.Sum(stat => stat.Value);

        /*******************************************************************/
        public void SetStat(Stat primaryStat, List<Stat> stats)
        {
            gameObject.SetActive(true);
            Stat = primaryStat;
            MultiStat = stats;
            _statableManager.Add(this);
            _value.text = TotalValue.ToString();
        }

        /*******************************************************************/
        Tween IStatable.UpdateValue() => DOTween.Sequence()
                  .Append(_value.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InCubic))
                  .InsertCallback(ViewValues.FAST_TIME_ANIMATION, () => _value.text = TotalValue.ToString())
                  .Append(_value.transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION * 0.75f).SetEase(Ease.OutBack, 3f));
    }
}
