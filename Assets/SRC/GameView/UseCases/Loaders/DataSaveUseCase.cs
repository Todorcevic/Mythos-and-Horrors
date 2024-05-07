using Zenject;

namespace MythosAndHorrors.GameView
{
    public class DataSaveUseCase
    {
        [InjectOptional] private readonly string _jsonSaveDataPath = "Assets/Data/Save/SaveData.json";
        [Inject] private readonly JsonService _jsonService;

        public DataSave DataSave { get; private set; }

        /*******************************************************************/
        public void Load()
        {
            DataSave = _jsonService.CreateDataFromFile<DataSave>(_jsonSaveDataPath);
        }

        public void Save(DataSave data)
        {
            _jsonService.SaveFileFromData(data, _jsonSaveDataPath);
        }
    }
}
