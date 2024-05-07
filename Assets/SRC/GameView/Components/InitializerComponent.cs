using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _executedByTests = true;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        private async void Start()
        {
            await IntialState();
            _prepareGameUseCase.Execute();
            if (!_executedByTests) return;
            await _gameActionsProvider.Create(new StartGameAction());
        }

        private async Task IntialState()
        {
            DOTween.SetTweensCapacity(500, 312);
            _mainButtonComponent.DeactivateToClick();
            await _ioActivatorComponent.DeactivateCardSensors();
        }
    }
}
