using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _normalExecution = true;
        [Inject] private readonly PrepareAllUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        private async void Start()
        {
            IntialState();
            _prepareGameUseCase.Execute();
            if (!_normalExecution) return;
            await _gameActionsProvider.Create<StartGameAction>().Execute();
        }

        private void IntialState()
        {
            DOTween.SetTweensCapacity(500, 312);
            _mainButtonComponent.DeactivateToClick();
        }
    }
}
