using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
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
            TurnOn();
        }

        private Tween TurnOn()
        {
            int amount = Stat.Value;
            AddExtraTurn(amount - _turns.Count);

            Sequence turningSequence = DOTween.Sequence();
            for (int i = 0; i < ActiveTurnsCount; i++)
            {
                turningSequence.Join(SwitchOffTurn(_turns[i]));
            }

            turningSequence.Append(DOTween.Sequence());
            for (int i = 0; i < amount; i++)
            {
                turningSequence.Join(SwitchOnTurn(_turns[i]));
            }

            return turningSequence;
        }

        private void AddExtraTurn(int amount)
        {
            if (amount <= 0) return;
            for (int i = 0; i < amount; i++)
            {
                GameObject newTurn = Instantiate(_turns.First(), transform);
                _turns.Add(newTurn);
                newTurn.SetActive(false);
                newTurn.transform.localScale = Vector3.zero;
            }
        }

        private Tween SwitchOnTurn(GameObject turn) => turn.transform.DOScale(1, ViewValues.FAST_TIME_ANIMATION)
                .OnStart(() => { turn.transform.localScale = Vector3.zero; turn.SetActive(true); });

        private Tween SwitchOffTurn(GameObject turn) =>
            turn.transform.DOScale(0, ViewValues.FAST_TIME_ANIMATION).OnComplete(() => turn.SetActive(false));

        Tween IStatableView.UpdateValue() => TurnOn();
    }
}
