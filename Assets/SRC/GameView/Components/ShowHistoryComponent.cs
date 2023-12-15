using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryComponent : MonoBehaviour
    {
        private TaskCompletionSource<bool> waitForClicked;
        [SerializeField, Required, SceneObjectsOnly] Transform _outPosition;
        [SerializeField, Required, SceneObjectsOnly] Transform _showPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _content;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _screen;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _button;

        /*******************************************************************/
        public async Task Show(History history)
        {
            waitForClicked = new();
            _title.text = history.Title;
            _content.text = history.Description;
            _screen.LoadHistorySprite(history.Image);
            await transform.DOMove(_showPosition.position, ViewValues.SLOW_TIME_ANIMATION).AsyncWaitForCompletion();
            await waitForClicked.Task;
            await transform.DOMove(_outPosition.position, ViewValues.SLOW_TIME_ANIMATION).AsyncWaitForCompletion();
        }

        public void Clicked()
        {
            waitForClicked.SetResult(true);
        }
    }
}