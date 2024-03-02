using Zenject;

namespace MythsAndHorrors.GameView
{
    public class DataSaveLoaderUseCase
    {
        private string JSON_SAVE_DATA_PATH => "Assets/Data/Save/SaveData.json";
        [Inject] private readonly JsonService _jsonService;

        public DataSave DataSave { get; private set; }

        public void Execute()
        {
            DataSave = _jsonService.CreateDataFromFile<DataSave>(JSON_SAVE_DATA_PATH);
        }
    }
}
