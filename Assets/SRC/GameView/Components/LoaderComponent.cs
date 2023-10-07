using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [InjectOptional][SerializeField] private bool _mustBeLoaded; //From SceneComponent in MasterScene
        [Inject] private readonly InitializeGameUseCase _initializeGameUseCase;

        /*******************************************************************/
        private async void Start()
        {
            if (!_mustBeLoaded) return;
            await _initializeGameUseCase.Execute();
        }
    }
}
