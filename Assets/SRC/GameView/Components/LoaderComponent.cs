using UnityEngine;
using Zenject;

namespace GameView
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
