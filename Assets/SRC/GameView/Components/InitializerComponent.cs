using MythsAndHorrors.EditMode;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _mustBeLoaded = true;
        [Inject] private readonly PrepareGameUseCase _loadGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        private async void Start()
        {
            if (!_mustBeLoaded) return;

            _loadGameUseCase.Execute();
            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
