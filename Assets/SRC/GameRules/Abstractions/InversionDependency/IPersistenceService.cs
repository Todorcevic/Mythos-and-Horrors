namespace MythsAndHorrors.GameRules
{
    public interface IPersistenceService
    {
        T CreateDataFromFile<T>(string pathAndNameJsonFile);
        T CreateDataFromResources<T>(string pathAndNameJsonFile);
        void SaveFileFromData(object data, string pathAndNameJsonFile);
        void UpdateDataFromFile(string pathAndNameJsonFile, object objectToUpdate);
        void UpdateDataFromResources(string pathAndNameJsonFile, object objectToUpdate);
    }
}