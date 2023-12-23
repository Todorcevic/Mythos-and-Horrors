using MythsAndHorrors.GameRules;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializerComponent : MonoBehaviour
    {
        [InjectOptional] private readonly bool _mustBeLoaded = true;
        [Inject] private readonly PrepareGameUseCase _loadGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        private async void Start()
        {
            if (!_mustBeLoaded) return;

            _loadGameUseCase.Execute();
            await _gameActionFactory.Create<StartChapterGameAction>().Run(_chaptersProvider.CurrentChapter);
            await _gameActionFactory.Create<StartGameAction>().Run();
        }
    }
}
