using UnityEngine;
using Zenject;

namespace Tuesday.GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly InitializeGameUseCase _initializeGameUseCase;

        /*******************************************************************/
        private async void Start()
        {
            await _initializeGameUseCase.Execute();
        }
    }
}
