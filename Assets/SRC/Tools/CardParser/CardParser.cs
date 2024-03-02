#pragma warning disable IDE0051, IDE0052 // Remove unused private members
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using FilePathAttribute = Sirenix.OdinInspector.FilePathAttribute;

namespace MythosAndHorrors.Tools
{

    public class CardParser : OdinEditorWindow
    {
        private const string DATA_PATH = "Assets/Data/";
        private const string FILE_EXTENSION = ".json";
        private readonly JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        private List<OldCardInfo> allCardData;
        private List<Pack> allPacks;

        private bool IsAllPacksLoaded => allPacks != null && allPacks.Count > 0;

        /*******************************************************************/
        [MenuItem("Tools/Cards/CardParser")]
        public static void Parser()
        {
            CardParser window = GetWindow<CardParser>();
            window.minSize = new Vector2(600, 600);
            window.position = new Rect((Screen.currentResolution.width - window.minSize.x) / 2,
                                       (Screen.currentResolution.height - window.minSize.y) / 2,
                                       window.minSize.x, window.minSize.y);
        }

        protected override void Initialize()
        {
            WindowPadding = new Vector4(16, 16, 16, 16);
        }

        /*******************************************************************/

        [FilePath(Extensions = ".json", ParentFolder = DATA_PATH)]
        [OnValueChanged("BuildCardInfo")]
        [SerializeField, LabelText("Create CardInfo json from oldCardInfo json file")]
        private string JSONFileLoaded;

        private void BuildCardInfo()
        {
            allCardData = JsonConvert.DeserializeObject<List<OldCardInfo>>(GetJsonData());
            SelectPathAndSave("cardInfo");

            /*******************************************************************/
            string GetJsonData()
            {
                StreamReader reader = new(DATA_PATH + JSONFileLoaded);
                string jsonData = reader.ReadToEnd();
                reader.Close();
                return jsonData;
            }
        }

        [PropertySpace(8)]
        [GUIColor("@Color.green")]
        [Button(ButtonSizes.Large, Name = "Load allPacks from Online")]
        private async void LoadAllPacks()
        {
            string url = "https://arkhamdb.com/api/public/packs/";
            try
            {
                using HttpClient client = new();
                var dataLoaded = await client.GetStringAsync(url);
                allPacks = JsonConvert.DeserializeObject<List<Pack>>(dataLoaded);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Cant load packs: {ex.Message}");
            }
        }

        [PropertySpace(8)]
        [ShowIf("IsAllPacksLoaded")]
        [GUIColor("@Color.green")]
        [Button(ButtonSizes.Large, Name = "Save json packs in json file")]
        private void SelectDetinationAndSave()
        {
            string path = EditorUtility.SaveFilePanel("Save JSON", DATA_PATH, "packs", "json");
            if (string.IsNullOrEmpty(path)) return;

            string jsonData = JsonConvert.SerializeObject(allPacks, Formatting.Indented, jsonSettings);
            SaveToFile(path, jsonData);
        }

        [ShowIf("IsAllPacksLoaded")]
        [GUIColor("@Color.green")]
        [Button(ButtonSizes.Large, Name = "Save individual packs in json files")]
        private async void SaveIndividualPacks()
        {
            string URL = "https://arkhamdb.com/api/public/cards/";
            string path = "Assets/Data/ArkhamDBOriginal/Packs/";
            try
            {
                using HttpClient client = new();
                foreach (Pack pack in allPacks)
                {
                    string jsonData = await client.GetStringAsync(URL + pack.code);
                    SaveToFile($"{path}{pack.code}.json", jsonData);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error: {ex.Message}");
            }
        }

        [ShowIf("IsAllPacksLoaded")]
        [GUIColor("@Color.green")]
        [Button(ButtonSizes.Large, Name = "Create CardInfo json packs loaded")]
        private async void Create()
        {
            string URL = "https://arkhamdb.com/api/public/cards/";
            try
            {
                allCardData = new();
                using HttpClient client = new();
                foreach (Pack pack in allPacks)
                {
                    string jsonResponse = await client.GetStringAsync(URL + pack.code);
                    allCardData.AddRange(JsonConvert.DeserializeObject<List<OldCardInfo>>(jsonResponse));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error: {ex.Message}");
            }

            SelectPathAndSave("cardInfo");
        }

        private void SelectPathAndSave(string fileName)
        {
            string path = EditorUtility.SaveFilePanel("Save JSON", DATA_PATH, fileName, "json");
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("No path selected");
                return;
            }
            string jsonData = Convert();
            SaveToFile(path, jsonData);

            string Convert()
            {
                List<CardInfo> allNewCards = new();
                allCardData.ForEach(oldCardInfo => allNewCards.Add(new CardInfo().CreateWith(oldCardInfo)));
                return JsonConvert.SerializeObject(allNewCards, Formatting.Indented, jsonSettings);
            }
        }

        private void SaveToFile(string path, string jsonData)
        {
            try
            {
                string directory = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(path, jsonData);
                Debug.Log($"Datos guardados en: {path}");
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar el archivo: {ex.Message}");
            }
        }
    }
}
#pragma warning restore IDE0051, IDE0052 // Remove unused private members
