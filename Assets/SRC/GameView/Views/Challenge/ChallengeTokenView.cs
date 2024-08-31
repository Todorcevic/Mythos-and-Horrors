using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeTokenView : MonoBehaviour
    {
        [SerializeField, Required] private ChallengeTokenType _type;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private Rigidbody _rigidBody;

        public ChallengeToken ChallengeToken { get; private set; }
        public ChallengeTokenType Type => _type;
        public bool IsValueToken => (int)_type < 10;

        /*******************************************************************/
        public Tween PushUp()
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(transform.up * Random.Range(100f, 200f), ForceMode.Impulse);
            _rigidBody.AddTorque(new Vector3(10, 20, 50), ForceMode.Impulse);
            return DotweenExtension.Wait(ViewValues.DEFAULT_TIME_ANIMATION * 2);
        }

        public Tween Restore(Transform centerShow, Transform ChallengeBag)
        {
            Sleep();
            return DOTween.Sequence().Join(transform.DOMove(centerShow.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Join(transform.DORotate(centerShow.eulerAngles, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Join(transform.DOScale(Vector3.one * 4, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .AppendInterval(ViewValues.DEFAULT_TIME_ANIMATION)
                     .Append(transform.DOFullLocalMove(ChallengeBag, ViewValues.DEFAULT_TIME_ANIMATION))
                     .OnComplete(() => Destroy(gameObject));
        }

        public Tween ShowCenter(Transform centerShow)
        {
            Transform realTransfromt = transform;
            return DOTween.Sequence().Join(transform.DOMove(centerShow.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Join(transform.DORotate(centerShow.eulerAngles, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Join(transform.DOScale(Vector3.one * 4, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Append(transform.DOFullMove(realTransfromt));
        }

        public Tween ShakeToken()
        {
            return transform.DOShakePosition(ViewValues.SLOW_TIME_ANIMATION, 1f, 10, 90, false, true);
        }

        public void SetValue(ChallengeToken challengeToken, Investigator investigator)
        {
            ChallengeToken = challengeToken;
            _value.text = challengeToken.Value(investigator).ToString();
            if (!IsValueToken) _value.DOFade(0, 0);
            else _value.DOFade(1, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine);
        }

        private void Sleep()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            _rigidBody.Sleep();
            _rigidBody.isKinematic = true;
        }
    }
}

