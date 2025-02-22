using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using Zenject;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class ResultInfoComponent : MonoBehaviour
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly TextsManager _textsProvider;
        [Inject] private readonly AudioComponent _audioComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _zero;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _resultMessage;
        [SerializeField, Required, ChildGameObjectsOnly] private GameObject _meter;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _background;
        [SerializeField, Required, AssetsOnly] private AudioClip _success;
        [SerializeField, Required, AssetsOnly] private AudioClip _fail;

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
            _resultMessage.text = isSuccessful ?
                _textsProvider.GetLocalizableText(new Localization("Challenge_Component_Succeed")) :
                _textsProvider.GetLocalizableText(new Localization("Challenge_Component_Fail"));
            return DOTween.Sequence().Join(_resultMessage.transform.DOScale(1, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(_background.DOColor(isSuccessful ? ViewValues.GREEN_FONT_COLOR2 : ViewValues.RED_FONT_COLOR2, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(_audioComponent.DOPlayAudio(isSuccessful ? _success : _fail))
                .SetEase(Ease.OutBack);
        }

        private void HideAll()
        {
            _resultMessage.gameObject.SetActive(false);
            _meter.SetActive(false);
            _background.color = new(1f, 1f, 1f);
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

