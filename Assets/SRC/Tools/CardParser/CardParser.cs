#pragma warning disable IDE0051, IDE0052 // Remove unused private members
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using FilePathAttribute = Sirenix.OdinInspector.FilePathAttribute;

namespace MythsAndHorrors.Tools
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

        private string FullPathLoaded => DATA_PATH + JSONFileLoaded;
        private bool IsJSONLoaded => File.Exists(FullPathLoaded);

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
        [SerializeField, LabelText("Load JSON file")]
        private string JSONFileLoaded;

        private void BuildCardInfo()
        {
            StreamReader reader = new(FullPathLoaded);
            string jsonData = reader.ReadToEnd();
            reader.Close();

            allCardData = JsonConvert.DeserializeObject<List<OldCardInfo>>(jsonData);
        }

        [ShowIf("IsJSONLoaded")]
        [LabelText("SaveAs")]
        [GUIColor("@Color.green")]
        [Button(ButtonSizes.Large, Name = "Save as")]
        private void SaveAs()
        {
            string newPath = EditorUtility.SaveFilePanel("Save JSON", DATA_PATH, "New JSON", "json");
            if (string.IsNullOrEmpty(newPath)) return;
            Save(newPath);
        }

        private void Save(string path)
        {
            List<CardInfo> allNewCards = new();

            foreach (OldCardInfo oldCardInfo in allCardData)
            {
                allNewCards.Add(new CardInfo().CreateWith(oldCardInfo));
            }

            string serializeInfo = JsonConvert.SerializeObject(allNewCards, Formatting.Indented, jsonSettings);
            StreamWriter writer = new(path);
            writer.WriteLine(serializeInfo);
            writer.Close();
            AssetDatabase.Refresh();
        }
    }
}
#pragma warning restore IDE0051, IDE0052 // Remove unused private members
