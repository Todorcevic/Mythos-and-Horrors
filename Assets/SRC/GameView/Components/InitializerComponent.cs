using MythsAndHorrors.GameRules;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional][SerializeField] private bool _mustBeLoaded; //From SceneComponent in MasterScene
        [Inject] private readonly LoadGameUseCase _loadGameUseCase;
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
