using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class TurnsCostController : MonoBehaviour, IStatable
    {
        private readonly List<SpriteRenderer> allTurns = new();
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _fastTurnPrefab;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _turnPrefab;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Stat turnCostStat)
        {
            Stat = turnCostStat;
            UpdateAnimation();
        }

        public Tween UpdateAnimation()
        {
            allTurns.ForEach(turn => Destroy(turn.gameObject));
            allTurns.Clear();
            if (Stat.Value == 0) allTurns.Add(Instantiate(_fastTurnPrefab, transform));
            else
            {
                for (int i = 0; i < Stat.Value; i++)
                {
                    allTurns.Add(Instantiate(_turnPrefab, transform));
                }
            }

            allTurns.ForEach(turn => turn.gameObject.SetActive(true));
            return DOTween.Sequence();
        }
    }
}
