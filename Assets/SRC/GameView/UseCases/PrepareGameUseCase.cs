using MythsAndHorrors.EditMode;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class PrepareGameUseCase
    {
        [Inject] private readonly CardInfoLoaderUseCase _cardInfoLoader;
        [Inject] private readonly SceneLoaderUseCase _sceneLoader;
        [Inject] private readonly AdventurerLoaderUseCase _adventurerLoader;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;

        private SaveData _saveData;

        /*******************************************************************/
        public void Execute(SaveData saveData)
        {
            _saveData = saveData;
            LoadCardInfo();
            LoadAdventurers();
            LoadScene();
            InitializeZones();
            BuildCardViews();
        }

        private void LoadCardInfo() => _cardInfoLoader.Execute(FilesPath.JSON_CARDINFO_PATH);

        private void LoadAdventurers() =>
            _saveData.AdventurersSelected.ForEach(adventurerCode =>
            _adventurerLoader.Execute(FilesPath.JSON_ADVENTURER_PATH(adventurerCode)));

        private void LoadScene() => _sceneLoader.Execute(FilesPath.JSON_SCENE_PATH(_saveData.SceneSelected));

        private void InitializeZones() => _zoneViewsManager.Init();

        private void BuildCardViews() => _cardProvider.GetAllCards().ForEach(card => _cardGeneratorComponent.BuildCard(card));
    }
}
