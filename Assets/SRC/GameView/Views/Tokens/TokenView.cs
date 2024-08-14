using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class TokenView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _amount;
        [SerializeField, Required, ChildGameObjectsOnly] private GameObject _model;

        public bool IsActive { get; private set; }

        /*******************************************************************/
        private void Activate()
        {
            IsActive = true;
            _model.SetActive(true);
        }

        public void Deactivate()
        {
            IsActive = false;
            _model.SetActive(false);
        }

        public Tween MoveTo(Transform destiny, Transform centerShow)
        {
            return DOTween.Sequence()
                   .OnStart(Starting)
                   .Append(MoveCenter(centerShow))
                   .Append(_model.transform.DORecolocate(ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)))
                   .OnComplete(ReturnToken);

            void Starting()
            {
                _amount.transform.localScale = Vector3.zero;
                _model.transform.SetParent(destiny);
            }

            void ReturnToken()
            {
                Deactivate();
                _model.transform.SetParent(transform);
            }
        }

        public Tween MoveFrom(Transform origin, Transform centerShow)
        {
            return DOTween.Sequence()
                .OnStart(PositionateToken)
                .Append(MoveCenter(centerShow))
                .Append(_model.transform.DORecolocate(ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)));

            void PositionateToken()
            {
                _amount.transform.localScale = Vector3.zero;
                _model.transform.SetEqual(origin);
                Activate();
            }
        }

        public Tween SetAmount(int amount)
        {
            return DOTween.Sequence()
                 .Append(_amount.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InCubic))
                 .InsertCallback(ViewValues.FAST_TIME_ANIMATION, () => _amount.text = amount > 1 ? amount.ToString() : string.Empty)
                 .Append(_amount.transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION * 0.75f).SetEase(Ease.OutBack, 3f));
        }

        public TokenView Clone()
        {
            TokenView newToken = Instantiate(this, transform.parent);
            newToken._amount.text = string.Empty;
            newToken.name = name;
            newToken.transform.position += Vector3.up * 0.2f;
            return newToken;
        }

        private Tween MoveCenter(Transform centerShow)
        {
            Vector3 randomPosition = (Random.insideUnitSphere * 5f) + Vector3.up * Random.Range(-0.2f, 0.2f);
            return DOTween.Sequence()
                 .Append(_model.transform.DOMove(centerShow.position + randomPosition, ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)).SetEase(Ease.OutCubic))
                 .Join(_model.transform.DOScale(2f, ViewValues.FAST_TIME_ANIMATION))
                 .Join(_model.transform.DORotate(centerShow.rotation.eulerAngles, ViewValues.FAST_TIME_ANIMATION * Random.Range(1.5f, 2.5f)).SetEase(Ease.Linear))
                 .Append(_model.transform.DORotate(centerShow.rotation.eulerAngles + new Vector3(0f, 0, -0.5f), 0.1f));
        }
    }
}
