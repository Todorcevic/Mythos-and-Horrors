using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TurnController : MonoBehaviour, IStatableView
    {
        [SerializeField, Required] private List<GameObject> _turns;

        public int ActiveTurnsCount => _turns.Count(turn => turn.activeSelf);

        public Stat Stat { get; private set; }

        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Stat stat)
        {
            Stat = stat;
            TurnOn(stat.Value);
        }

        public void TurnOn(int amount)
        {
            AddTurn(amount - _turns.Count);
            _turns.ForEach(turn => turn.SetActive(false));
            for (int i = 0; i < amount; i++)
            {
                _turns[i].SetActive(true);
            }
        }

        private void AddTurn(int amount)
        {
            if (amount <= 0) return;
            for (int i = 0; i < amount; i++)
            {
                _turns.Add(Instantiate(_turns.First(), transform));
            }
        }

        Tween IStatableView.UpdateValue()
        {
            TurnOn(Stat.Value);
            return DOTween.Sequence();
        }
    }
}
