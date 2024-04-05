using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Reflection;
using System;
using TMPro;
using UnityEngine;
using Zenject;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class ChallengeMeterComponent : MonoBehaviour
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _zero;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus2;

        /*******************************************************************/
        public void Show()
        {
            ChallengePhaseGameAction currentChallenge = _challengeTokensProvider.CurrentChallenge;
            _minos2.text = Calculate(currentChallenge, -2).ToString() + "%";
            _minos1.text = Calculate(currentChallenge, -1).ToString() + "%";
            _zero.text = Calculate(currentChallenge, 0).ToString() + "%";
            _plus1.text = Calculate(currentChallenge, 1).ToString() + "%";
            _plus2.text = Calculate(currentChallenge, 2).ToString() + "%";
        }

        private double Calculate(ChallengePhaseGameAction challenge, int mod)
        {
            int amountWins = _challengeTokensProvider.ChallengeTokens.Count(token =>
                token.TokenType != ChallengeTokenType.Fail &&
                challenge.TotalChallengeValue + token.Value() + mod >= challenge.DifficultValue);

            return Math.Round(((double)amountWins / _challengeTokensProvider.Tokens.Count) * 100);
        }
    }
}

