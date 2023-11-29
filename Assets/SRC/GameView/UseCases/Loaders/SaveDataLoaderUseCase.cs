using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SaveDataLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        public SaveData Execute() => _jsonService.CreateDataFromFile<SaveData>(_filesPath.JSON_SAVE_DATA_PATH);

    }
}
