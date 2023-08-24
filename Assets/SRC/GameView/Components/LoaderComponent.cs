using GameRules;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly InitializeGameUseCase _initializeGameUseCase;
        [Inject] private readonly GameActionRepository _gameActionRepository;

        /*******************************************************************/
        private async void Start()
        {
            _initializeGameUseCase.Execute();
            await _gameActionRepository.GiveMe<StartGameAction>().Run();
        }
    }
}
