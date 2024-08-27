using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class JsonService
    {
        [Inject] private readonly CardConverter _cardConverter;
        [Inject] private readonly StatConverter _statConverter;
        [Inject] private readonly StateConverter _stateConverter;

        private JsonConverter[] AllConverters => new JsonConverter[] { _cardConverter, _statConverter, _stateConverter };

        /*******************************************************************/
        public JsonService()
        {
            JsonConvert.DefaultSettings = () => new()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };
        }

        /*******************************************************************/
        public T CreateDataFromResources<T>(string pathAndNameJsonFile)
        {
            TextAsset jsonData = Resources.Load<TextAsset>(pathAndNameJsonFile);
            return JsonConvert.DeserializeObject<T>(jsonData.text, AllConverters);
        }

        public object CreateDataFromFile(Type type, string pathAndNameJsonFile)
        {
            string jsonData = File.ReadAllText(pathAndNameJsonFile);
            return JsonConvert.DeserializeObject(jsonData, type, AllConverters);
        }

        public T CreateDataFromFile<T>(string pathAndNameJsonFile)
        {
            string jsonData = File.ReadAllText(pathAndNameJsonFile);
            return JsonConvert.DeserializeObject<T>(jsonData, AllConverters);
        }

        public void SaveFileFromData(object data, string pathAndNameJsonFile)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, AllConverters);
            File.WriteAllText(pathAndNameJsonFile, jsonData);
        }

        public void UpdateDataFromResources(string pathAndNameJsonFile, object objectToUpdate)
        {
            TextAsset jsonData = Resources.Load<TextAsset>(pathAndNameJsonFile);
            JsonConvert.PopulateObject(jsonData.text, objectToUpdate);
        }

        public void UpdateDataFromFile(string pathAndNameJsonFile, object objectToUpdate)
        {
            string jsonData = File.ReadAllText(pathAndNameJsonFile);
            JsonConvert.PopulateObject(jsonData, objectToUpdate);
        }
    }
}
