using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class RegisterChapterComponent : MonoBehaviour
    {
        private TaskCompletionSource<bool> waitForClicked;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        [SerializeField, Required, SceneObjectsOnly] Transform _outPosition;
        [SerializeField, Required, SceneObjectsOnly] Transform _showPosition;
        [SerializeField, Required, ChildGameObjectsOnly] List<RegisterInvestigatorController> _investigatorController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _registerScene;
        [SerializeField, Required, SceneObjectsOnly] Image _blockBackground;
        [SerializeField, Required, ChildGameObjectsOnly] private ScrollRect _scrollRect;
        [SerializeField, Required, ChildGameObjectsOnly] private Button _button;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Initialized by Injection")]
        private void Init()
        {
            _button.onClick.AddListener(Clicked);
            _button.interactable = false;
            _registerScene.text = string.Empty;
        }

        /*******************************************************************/
        public async Task Show()
        {
            waitForClicked = new();
            ShowInvestigators();
            ShowRegister();
            _button.interactable = true;
            await ShowAnimation().AsyncWaitForCompletion();
            await waitForClicked.Task;
            _button.interactable = false;
            await HideAnimation().AsyncWaitForCompletion();
        }

        private void ShowInvestigators()
        {
            int i = 0;
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                _investigatorController[i].SetInvestigator(investigator);
                i++;
            }
        }

        private void ShowRegister()
        {
            foreach (var register in _chaptersProvider.CurrentChapter.Register)
            {
                _registerScene.text += $"* {_chaptersProvider.CurrentChapter.RegisterEnum.GetEnumName(register.Key)} - {register.Value}\n";
            }
        }

        private Sequence ShowAnimation() => DOTween.Sequence()
               .Join(_scrollRect.DOVerticalNormalizedPos(1f, ViewValues.DEFAULT_TIME_ANIMATION))
               .Join(_blockBackground.DOFade(ViewValues.DEFAULT_FADE, ViewValues.DEFAULT_TIME_ANIMATION))
               .Join(transform.DOMove(_showPosition.position, ViewValues.DEFAULT_TIME_ANIMATION)
               .SetEase(Ease.OutBack, 1.1f));

        private Sequence HideAnimation() => DOTween.Sequence()
               .Join(_blockBackground.DOFade(0f, ViewValues.DEFAULT_TIME_ANIMATION))
               .Join(transform.DOMove(_outPosition.position, ViewValues.DEFAULT_TIME_ANIMATION))
               .SetEase(Ease.InOutCubic);

        private void Clicked()
        {
            waitForClicked.SetResult(true);
        }
    }
}
