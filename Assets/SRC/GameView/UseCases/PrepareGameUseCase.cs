using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PrepareGameUseCase
    {
        [Inject] private readonly ChapterInfoLoaderUseCase _chapterInfoLoaderUseCase;
        [Inject] private readonly SceneLoaderUseCase _sceneLoaderUseCase;
        [Inject] private readonly InvestigatorLoaderUseCase _investigatorLoaderUseCase;
        [Inject] private readonly SaveDataLoaderUseCase saveDataLoaderUseCase;
        [Inject] private readonly ZoneLoaderUseCase _zoneLoaderUseCase;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;
        [Inject] private readonly FilesPath _filesPath;
        private SaveData _saveData;

        /*******************************************************************/
        public void Execute()
        {
            LoadSaveData();
            LoadInvestigators();
            LoadChapters();
            LoadScene();
            LoadZones();
            BuildCardViews();
        }

        private void LoadSaveData() => _saveData = saveDataLoaderUseCase.Execute();

        private void LoadInvestigators() =>
            _saveData.InvestigatorsSelected.ForEach(investigatorCode =>
            _investigatorLoaderUseCase.Execute(_filesPath.JSON_INVESTIGATOR_PATH(investigatorCode)));

        private void LoadChapters() =>
            _chapterInfoLoaderUseCase.Execute(_filesPath.JSON_CHAPTERINFO_PATH, _saveData.DificultySelected);

        private void LoadScene() => _sceneLoaderUseCase.Execute(_filesPath.JSON_SCENE_PATH(_saveData.SceneSelected));

        private void LoadZones() => _zoneLoaderUseCase.Execute();

        private void BuildCardViews() => _cardProvider.AllCards.ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
