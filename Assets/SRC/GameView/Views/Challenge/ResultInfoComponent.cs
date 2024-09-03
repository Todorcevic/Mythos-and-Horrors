using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using Zenject;
using System.Linq;
using DG.Tweening;

namespace MythosAndHorrors.GameView
{
    public class ResultInfoComponent : MonoBehaviour
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _zero;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _resultMessage;
        [SerializeField, Required, ChildGameObjectsOnly] private GameObject _meter;

        /*******************************************************************/
        public Tween Show(ChallengePhaseGameAction ChallengePhaseGameAction)
        {
            HideAll();
            bool? isSuccessful = ChallengePhaseGameAction.ResultChallenge?.IsSuccessful;
            if (isSuccessful.HasValue) return ShowResultMessage(isSuccessful.Value);
            else return ShowMeter(ChallengePhaseGameAction);
        }

        private Tween ShowMeter(ChallengePhaseGameAction ChallengePhaseGameAction)
        {
            _meter.SetActive(true);
            _minos2.text = Calculate(ChallengePhaseGameAction, -2).ToString() + "%";
            _minos1.text = Calculate(ChallengePhaseGameAction, -1).ToString() + "%";
            _zero.text = Calculate(ChallengePhaseGameAction, 0).ToString() + "%";
            _plus1.text = Calculate(ChallengePhaseGameAction, 1).ToString() + "%";
            _plus2.text = Calculate(ChallengePhaseGameAction, 2).ToString() + "%";
            return DOTween.Sequence();
        }

        private Tween ShowResultMessage(bool isSuccessful)
        {
            _resultMessage.transform.DOScale(0, 0).SetEase(Ease.InBack);
            _resultMessage.gameObject.SetActive(true);
            _resultMessage.fontMaterial.EnableKeyword(ShaderUtilities.Keyword_Glow);
            _resultMessage.fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, isSuccessful ? ViewValues.GREEN_FONT_COLOR : ViewValues.RED_FONT_COLOR);
            _resultMessage.text = isSuccessful ?
                _textsProvider.GetLocalizableText(new Localization("Challenge_Component_Succeed")) :
                _textsProvider.GetLocalizableText(new Localization("Challenge_Component_Fail"));
            return _resultMessage.transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack);
        }

        private void HideAll()
        {
            _resultMessage.gameObject.SetActive(false);
            _meter.SetActive(false);
        }

        private double Calculate(ChallengePhaseGameAction challenge, int mod)
        {
            int amountWins = _challengeTokensProvider.ChallengeTokensInBag.Count(token =>
                token.TokenType != ChallengeTokenType.Fail &&
                challenge.CurrentTotalChallengeValue + token.Value(challenge.ActiveInvestigator) + mod >= challenge.DifficultValue);

            return Math.Round(((double)amountWins / _challengeTokensProvider.ChallengeTokensInBag.Count) * 100);
        }
    }
}

