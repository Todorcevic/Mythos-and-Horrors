using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryComponent : MonoBehaviour
    {
        private Vector3 initialScale;

        private TaskCompletionSource<bool> waitForClicked;
        [SerializeField, Required, SceneObjectsOnly] Transform _outPosition;
        [SerializeField, Required, SceneObjectsOnly] Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] Image _blockBackground;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _content;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _screen;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _button;
        [SerializeField, Required, ChildGameObjectsOnly] private ScrollRect _scrollRect;

        /*******************************************************************/
        private void Start()
        {
            initialScale = transform.localScale;
            _button.onClick.AddListener(Clicked);
        }

        /*******************************************************************/
        public async Task Show(History history, Transform worldObject = null)
        {
            waitForClicked = new();
            _title.text = history.Title;
            _content.text = history.Description;
            _button.interactable = true;
            transform.localScale = Vector3.zero;
            Vector3 returnPosition = transform.position = (worldObject == null) ? _outPosition.transform.position : RectTransformUtility.WorldToScreenPoint(Camera.main, worldObject.transform.TransformPoint(Vector3.zero));

            await _screen.LoadHistorySprite(history.Image);
            await ShowAnimation().AsyncWaitForCompletion();
            await waitForClicked.Task;
            _button.interactable = false;
            await HideAnimation(returnPosition).AsyncWaitForCompletion();
        }

        private void Clicked()
        {
            waitForClicked.SetResult(true);
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .Join(_scrollRect.DOVerticalNormalizedPos(1f, ViewValues.SLOW_TIME_ANIMATION))
                .Join(_blockBackground.DOFade(ViewValues.DEFAULT_FADE, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DOMove(_showPosition.position, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()
                .Join(_blockBackground.DOFade(0f, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DOMove(returnPosition, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.SLOW_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);
    }
}