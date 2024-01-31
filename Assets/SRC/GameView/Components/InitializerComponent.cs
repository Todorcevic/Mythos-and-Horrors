using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _mustBeLoaded = true;
        [Inject] private readonly PrepareGameUseCase _loadGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        private async void Start()
        {
            if (!_mustBeLoaded) return;
            await IntialState();
            _loadGameUseCase.Execute();
            await _gameActionFactory.Create(new StartChapterGameAction(_chaptersProvider.CurrentChapter));
            await _gameActionFactory.Create(new StartGameAction());
            await _gameActionFactory.Create(new InteractableGameAction(false));
        }

        private async Task IntialState()
        {
            DOTween.SetTweensCapacity(200, 125);
            _ioActivatorComponent.BlockUI();
            _mainButtonComponent.Deactivate();
            await _ioActivatorComponent.DeactivateCardSensors();
        }
    }
}
