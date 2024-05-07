using Zenject;

namespace MythosAndHorrors.GameView
{
    public class DataSaveUseCase
    {
        private string JSON_SAVE_DATA_PATH => "Assets/Data/Save/SaveData.json";
        [Inject] private readonly JsonService _jsonService;

        public DataSave DataSave { get; private set; }

        public void Load()
        {
            DataSave = _jsonService.CreateDataFromFile<DataSave>(JSON_SAVE_DATA_PATH);
        }

        public void Save(DataSave data)
        {
            _jsonService.SaveFileFromData(data, JSON_SAVE_DATA_PATH);
        }
    }
}
