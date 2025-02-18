using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeToken3DView : MonoBehaviour
    {
        private AudioClip _tokenSound;
        [SerializeField, Required] private ChallengeTokenType _type;
        [SerializeField, Required, ChildGameObjectsOnly] private Rigidbody _rigidBody;
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _renderer;
        [SerializeField, Required, AssetsOnly] private List<AudioClip> _tableHits;
        [SerializeField, Required, AssetsOnly] private List<AudioClip> _tokenHits;
        [SerializeField, Required, AssetsOnly] private AudioClip _moveCenter;
        [SerializeField, Required, AssetsOnly] private AudioClip _return;
        [Inject] private readonly AudioComponent _audioComponent;

        public ChallengeToken ChallengeToken { get; private set; }
        public ChallengeTokenType Type => _type;
        public bool IsValueToken => (int)_type < 10;

        /*******************************************************************/
        public void SetValue(ChallengeToken challengeToken, Texture texture, AudioClip audio)
        {
            ChallengeToken = challengeToken;
            _renderer.material.mainTexture = texture;
            _tokenSound = audio;
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
            return DOTween.Sequence().OnStart(() => _audioComponent.PlayAudio(_moveCenter))
                     .Join(transform.DOMove(centerShow.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Join(transform.DORotate(centerShow.eulerAngles, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Join(transform.DOScale(Vector3.one * 4, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                     .Append(transform.DOFullLocalMove(ChallengeBag, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.InSine).OnStart(() => _audioComponent.PlayAudio(_return)))
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

        public Tween ShakeToken(Transform centerShow)
        {
            return DOTween.Sequence().OnStart(() => Starting())
                .Join(transform.DOMoveY(10, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutSine))
                .Join(transform.DORotate(centerShow.eulerAngles, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.OutSine))
                .Append(transform.DOShakePosition(ViewValues.SLOW_TIME_ANIMATION * 2, 1f, 10, 90, false, true))
                .Join(_audioComponent.DOPlayAudio(_tokenSound))
                .OnComplete(() => Finishing());

            /*******************************************************************/
            void Starting()
            {
                _rigidBody.isKinematic = true;
            }

            void Finishing()
            {
                _rigidBody.isKinematic = false;
            }
        }

        private void Sleep()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            _rigidBody.Sleep();
            _rigidBody.isKinematic = true;
        }

        /*******************************************************************/
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Table"))
                _audioComponent.PlayAudio(_tableHits[Random.Range(0, _tableHits.Count)], volume: Mathf.Clamp(collision.relativeVelocity.magnitude / 2.5f, 0.2f, 0.8f));
            if (collision.gameObject.CompareTag("Token"))
                _audioComponent.PlayAudio(_tokenHits[Random.Range(0, _tokenHits.Count)], volume: Mathf.Clamp(collision.relativeVelocity.magnitude / 2.5f, 0, 1));
        }
    }
}

