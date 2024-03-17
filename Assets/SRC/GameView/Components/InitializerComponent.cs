using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _mustBeLoaded = true;
        [Inject] private readonly PrepareGameUseCase _loadGameUseCase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        private async void Start()
        {
            if (!_mustBeLoaded) return;
            await IntialState();
            _loadGameUseCase.Execute();
     
            await _gameActionsProvider.Create(new StartGameAction());
        }

        private async Task IntialState()
        {
            DOTween.SetTweensCapacity(500, 312);
            _ioActivatorComponent.BlockUI();
            _mainButtonComponent.DeactivateToClick();
            await _ioActivatorComponent.DeactivateCardSensors();
        }
    }
}
