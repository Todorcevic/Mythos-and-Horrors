using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokenView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _amount;
        [SerializeField, Required, ChildGameObjectsOnly] private GameObject _model;

        public bool IsActive => _model.activeSelf;
        /*******************************************************************/
        public void Activate()
        {
            _model.SetActive(true);
        }

        public void Deactivate()
        {
            _model.SetActive(false);
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
            newToken.Deactivate();
            newToken.name = name;
            newToken.transform.position += Vector3.up * 0.2f;
            return newToken;
        }
    }
}
