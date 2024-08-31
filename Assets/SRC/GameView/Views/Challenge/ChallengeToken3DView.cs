using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeToken3DView : MonoBehaviour
    {
        [SerializeField, Required] private ChallengeTokenType _type;
        [SerializeField, Required, ChildGameObjectsOnly] private Rigidbody _rigidBody;
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _renderer;

        public ChallengeToken ChallengeToken { get; private set; }
        public ChallengeTokenType Type => _type;
        public bool IsValueToken => (int)_type < 10;

        /*******************************************************************/
        public void SetValue(ChallengeToken challengeToken, Texture texture)
        {
            ChallengeToken = challengeToken;
            _renderer.material.mainTexture = texture;
        }

        /*******************************************************************/
        public Tween PushUp()
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(transform.up * Random.Range(100f, 200f), ForceMode.Impulse);
            _rigidBody.AddTorque(new Vector3(10, 20, 50), ForceMode.Impulse);
            return DotweenExtension.Wait(ViewValues.DEFAULT_TIME_ANIMATION * 2);
        }

        public Tween RestoreAndDestroy(Transform centerShow, Transform ChallengeBag)
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

        private void Sleep()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            _rigidBody.Sleep();
            _rigidBody.isKinematic = true;
        }
    }
}

