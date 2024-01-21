using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private TurnView _turnPrefab;
        private readonly List<TurnView> _turns = new();

        /*******************************************************************/
        public void TurnOn(int amount)
        {
            AddTurn(amount - _turns.Count);
            _turns.ForEach(turn => turn.TurnOff());
            for (int i = 0; i < amount; i++)
            {
                _turns[i].TurnOn();
            }
        }

        private void AddTurn(int amount)
        {
            if (amount <= 0) return;
            for (int i = 0; i < amount; i++)
            {
                TurnView newTurnView = Instantiate(_turnPrefab, transform);
                _turns.Add(newTurnView);
            }
        }
    }
}
