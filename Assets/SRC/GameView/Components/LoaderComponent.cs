using GameRules;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly InitializeGameUseCase _initializeGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionRepository;

        /*******************************************************************/
        private async void Start()
        {
            _initializeGameUseCase.Execute();
            await _gameActionRepository.Create<StartGameAction>().Run();
        }
    }
}
