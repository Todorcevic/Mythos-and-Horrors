using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ActionController : MonoBehaviour, IStatable
    {
        private Stat _maxTurns;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ActionView> _actionViews;
        [Inject] private readonly StatableManager _statableManager;

        public int ActiveTurnsCount => _actionViews.FindAll(turn => turn.IsOn).Count;
        public Stat Stat { get; private set; }

        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Stat stat, Stat maxTurns)
        {
            Stat = stat;
            _maxTurns = maxTurns;
            _statableManager.Add(this);
            TurnOn();
        }

        private Tween TurnOn()
        {
            int amount = Stat.Value;
            CheckMaxTurn(amount);
            int amountToAdd = amount - ActiveTurnsCount;

            Sequence turningSequence = DOTween.Sequence();
            if (amountToAdd < 0)
            {
                for (int i = 0; i < -amountToAdd; i++)
                {
                    turningSequence.Join(GetActiveTurn().SwitchOff());
                }
            }
            else if (amountToAdd > 0)
            {
                for (int i = 0; i < amountToAdd; i++)
                {
                    turningSequence.Join(GetFreeTurn().SwitchOn());
                }
            }
            return turningSequence;
        }

        private void CheckMaxTurn(int realAmount)
        {
            int amount = (_maxTurns.Value > realAmount ? _maxTurns.Value : realAmount) - _actionViews.Count;
            if (amount < 0)
            {
                for (int i = 0; i < -amount; i++)
                {
                    ActionView turn = _actionViews.Last();
                    _actionViews.Remove(turn);
                    Destroy(turn.gameObject);
                }
            }
            else if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    ActionView newTurn = ZenjectHelper.Instantiate(GetFreeTurn() ?? GetActiveTurn(), transform);
                    _actionViews.Add(newTurn);
                }
            }
        }

        private ActionView GetFreeTurn() => _actionViews.FirstOrDefault(turn => !turn.IsOn);

        private ActionView GetActiveTurn() => _actionViews.Last(turn => turn.IsOn);

        Tween IStatable.UpdateAnimation() => TurnOn();
    }
}

