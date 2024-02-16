using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryComponent : MonoBehaviour, IShowHistory
    {
        private TaskCompletionSource<bool> waitForClicked;
        [SerializeField, Required, SceneObjectsOnly] Transform _outPosition;
        [SerializeField, Required, SceneObjectsOnly] Transform _showPosition;
        [SerializeField, Required, SceneObjectsOnly] Image _blockBackground;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _content;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _screen;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _button;

        /*******************************************************************/
        private void Start()
        {
            _button.onClick.AddListener(Clicked);
        }

        /*******************************************************************/
        public async Task Show(History history)
        {
            waitForClicked = new();
            _title.text = history.Title;
            _content.text = history.Description;
            _button.interactable = true;
            await _screen.LoadHistorySprite(history.Image);
            await ShowAnimation().AsyncWaitForCompletion();
            await waitForClicked.Task;
            _button.interactable = false;
            await HideAnimation().AsyncWaitForCompletion();
        }

        private void Clicked()
        {
            waitForClicked.SetResult(true);
        }

        private Tween ShowAnimation() => DOTween.Sequence()
                .Join(_blockBackground.DOFade(ViewValues.DEFAULT_FADE, ViewValues.MID_TIME_ANIMATION))
                .Join(transform.DOMove(_showPosition.position, ViewValues.MID_TIME_ANIMATION)).SetEase(Ease.OutBack, 1.5f);

        private Tween HideAnimation() => DOTween.Sequence()
                .Join(_blockBackground.DOFade(0f, ViewValues.MID_TIME_ANIMATION))
                .Join(transform.DOMove(_outPosition.position, ViewValues.MID_TIME_ANIMATION)).SetEase(Ease.InOutCubic);
    }
}