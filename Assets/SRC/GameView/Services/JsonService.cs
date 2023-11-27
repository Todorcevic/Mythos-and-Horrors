using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class JsonService
    {
        [Inject] private readonly CardConverter _converters;

        /*******************************************************************/
        public JsonService()
        {
            JsonConvert.DefaultSettings = () => new()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = new List<JsonConverter>() { _converters }
            };
        }

        /*******************************************************************/
        public T CreateDataFromResources<T>(string pathAndNameJsonFile)
        {
            TextAsset jsonData = Resources.Load<TextAsset>(pathAndNameJsonFile);
            return JsonConvert.DeserializeObject<T>(jsonData.text);
        }

        public T CreateDataFromFile<T>(string pathAndNameJsonFile)
        {
            string jsonData = File.ReadAllText(pathAndNameJsonFile);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public void SaveFileFromData(object data, string pathAndNameJsonFile)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
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
