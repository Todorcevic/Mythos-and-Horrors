using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PrepareGameUseCase
    {
        [Inject] private readonly DataSaveLoaderUseCase _dataSaveLoaderUseCase;
        [Inject] private readonly TextsLoaderUseCase _textsLoaderUseCase;
        [Inject] private readonly InvestigatorLoaderUseCase _investigatorLoaderUseCase;
        [Inject] private readonly ChapterInfoLoaderUseCase _chapterInfoLoaderUseCase;
        [Inject] private readonly SceneLoaderUseCase _sceneLoaderUseCase;
        [Inject] private readonly ZoneLoaderUseCase _zoneLoaderUseCase;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;

        /*******************************************************************/
        public void Execute()
        {
            _dataSaveLoaderUseCase.Execute();
            _textsLoaderUseCase.LoadGameTexts();
            _textsLoaderUseCase.LoadViewTexts();
            _investigatorLoaderUseCase.Execute();
            _chapterInfoLoaderUseCase.Execute();
            _sceneLoaderUseCase.Execute();
            _zoneLoaderUseCase.Execute();
            _cardGeneratorComponent.BuildAllCardViews();
        }
    }
}
