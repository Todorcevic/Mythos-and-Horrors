namespace GameRules
{
    public interface ISerializer
    {
        T CreateDataFromResources<T>(string pathAndNameJsonFile);
        T CreateDataFromFile<T>(string pathAndNameJsonFile);
    }
}
