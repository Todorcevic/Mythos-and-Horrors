using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowHistoryComponent : MonoBehaviour
    {
        private Vector3 initialScale;

        private TaskCompletionSource<bool> waitForClicked;
        [Inject] private readonly AudioComponent _audioComponent;
        [SerializeField, Required, SceneObjectsOnly] Transform _outPosition;
        [SerializeField, Required, SceneObjectsOnly] Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] Image _blockBackground;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _content;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _screen;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _button;
        [SerializeField, Required, ChildGameObjectsOnly] private ScrollRect _scrollRect;
        [SerializeField, Required, AssetsOnly] private AudioClip _showAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hideAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _clickedAudio;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Initialized by Injection")]
        private void Init()
        {
            initialScale = transform.localScale;
            _button.onClick.AddListener(Clicked);
            _button.interactable = false;
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
            _audioComponent.PlayAudio(_clickedAudio);
            waitForClicked.SetResult(true);
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
                .OnStart(() => _audioComponent.PlayAudio(_showAudio))
                .Join(_scrollRect.DOVerticalNormalizedPos(1f, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(_blockBackground.DOFade(ViewValues.DEFAULT_FADE, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f))
                .Join(transform.DOScale(initialScale, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation(Vector3 returnPosition) => DOTween.Sequence()
                .OnStart(() => _audioComponent.PlayAudio(_hideAudio))
                .Join(_blockBackground.DOFade(0f, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOMove(returnPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
                .SetEase(Ease.InOutCubic);
    }
}