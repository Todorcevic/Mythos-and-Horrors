using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PrepareGameRulesUseCase
    {
        [Inject] private readonly TextsLoaderUseCase _textsLoaderUseCase;
        [Inject] private readonly DataSaveUseCase _dataSaveLoaderUseCase;
        [Inject] private readonly ChapterInfoLoaderUseCase _chapterInfoLoaderUseCase;
        [Inject] private readonly SceneLoaderUseCase _sceneLoaderUseCase;
        [Inject] private readonly InvestigatorLoaderUseCase _investigatorLoaderUseCase;

        /*******************************************************************/
        public void Execute()
        {
            _dataSaveLoaderUseCase.Load();
            _textsLoaderUseCase.LoadGameTexts();
            _investigatorLoaderUseCase.Execute();
            _chapterInfoLoaderUseCase.Execute();
            _sceneLoaderUseCase.Execute();
        }
    }
}
