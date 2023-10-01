using Tuesday.GameRules;
using UnityEngine;
using Zenject;

namespace Tuesday.GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly InitializeGameUseCase _initializeGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        private async void Start()
        {
            _initializeGameUseCase.Execute();
            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
