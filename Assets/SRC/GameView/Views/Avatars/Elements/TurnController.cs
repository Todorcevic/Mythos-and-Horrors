using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class TurnController : MonoBehaviour, IStatableView
    {
        [SerializeField, Required] private List<GameObject> _turns;
        [Inject] private readonly StatableManager _statableManager;

        public int ActiveTurnsCount => _turns.Count(turn => turn.activeSelf);
        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Stat stat)
        {
            Stat = stat;
            _statableManager.Add(this);
            TurnOn(stat.Value);
        }

        private void TurnOn(int amount)
        {
            AddExtraTurn(amount - _turns.Count);
            _turns.ForEach(turn => turn.SetActive(false));
            for (int i = 0; i < amount; i++)
            {
                _turns[i].SetActive(true);
            }
        }

        private void AddExtraTurn(int amount)
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
            return DOTween.Sequence(); //TODO: Do animation
        }
    }
}
