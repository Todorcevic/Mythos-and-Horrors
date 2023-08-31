#pragma warning disable IDE0051, IDE0052 // Remove unused private members
using GameRules;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using FilePathAttribute = Sirenix.OdinInspector.FilePathAttribute;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

namespace Tools
{
    public class CardCreator : OdinEditorWindow
    {
        private const string DATA_PATH = "Assets/Data/";
        private const string ASSEMBLY_DATA = "GameRules";
        private string newFileNameinfoBoxMessage;
        private List<DataCreatorBase> allCardData;

        private string FullPathLoaded => DATA_PATH + JSONFileLoaded;
        private bool IsStructDataLoaded => structDataLoaded != null;
        private bool IsJSONLoaded => !string.IsNullOrEmpty(JSONFileLoaded);
        private bool IsCardSelected => cardSelected != null;

        /*******************************************************************/
        [MenuItem("Tools/Cards/Card Creator")]
        private static void Open()
        {
            CardCreator window = GetWindow<CardCreator>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 500);
        }

        protected override void Initialize()
        {
            WindowPadding = new Vector4(16, 16, 16, 16);
        }

        /*******************************************************************/
        [HideIf("IsStructDataLoaded")]
        [BoxGroup("Struct Data", ShowLabel = false)]
        [TypeFilter("GetCreatorDataTypes")]
        [OnValueChanged("@structDataType = structDataLoaded.GetType().ToString()")]
        [SerializeField, LabelText("Load Struct Data")]
        private DataCreatorBase structDataLoaded;
        private IEnumerable<Type> GetCreatorDataTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract
                    && !type.IsGenericTypeDefinition
                    && typeof(DataCreatorBase).IsAssignableFrom(type));
        }

        [ShowIfGroup("IsStructDataLoaded")]
        [BoxGroup("IsStructDataLoaded/Struct Data", ShowLabel = false)]
        [ReadOnly, SerializeField, LabelText("Struct Loaded")]
        private string structDataType;

        /*******************************************************************/
        [ShowIfGroup("IsStructDataLoaded")]
        [BoxGroup("IsStructDataLoaded/JSON File", ShowLabel = false)]
        [HorizontalGroup("IsStructDataLoaded/JSON File/FileGroup", LabelWidth = 100)]
        [InfoBox("$newFileNameinfoBoxMessage", InfoMessageType.Warning, "@ !string.IsNullOrEmpty(newFileNameinfoBoxMessage)")]
        [SerializeField, LabelText("New JSON file")]
        private string newJSONFileName;

        [BoxGroup("IsStructDataLoaded/JSON File")]
        [HorizontalGroup("IsStructDataLoaded/JSON File/FileGroup", Width = 16, LabelWidth = 100)]
        [Button("+")]
        private void CreateFile()
        {
            newFileNameinfoBoxMessage = string.Empty;
            if (string.IsNullOrEmpty(newJSONFileName))
            {
                newFileNameinfoBoxMessage = "File name cant be empty!";
                return;
            }

            JSONFileLoaded = newJSONFileName + ".json";
            if (File.Exists(FullPathLoaded))
            {
                newFileNameinfoBoxMessage = "File exist!";
                return;
            }

            File.WriteAllText(FullPathLoaded, string.Empty);
            structDataLoaded.IsEditable = true;
            allCardData = new();
            ShowAllCardInfoLoaded();
            AssetDatabase.Refresh();
        }

        [BoxGroup("IsStructDataLoaded/JSON File")]
        [OnValueChanged("LoadData")]
        [HorizontalGroup("IsStructDataLoaded/JSON File/Other", LabelWidth = 100)]
        [FilePath(Extensions = ".json", ParentFolder = DATA_PATH)]
        [SerializeField, LabelText("Load JSON file")]
        private string JSONFileLoaded;
        private void LoadData()
        {
            if (string.IsNullOrEmpty(FullPathLoaded) || !File.Exists(FullPathLoaded)) return;

            allCardData = ReadData();
            Debug.Log(allCardData.Count + " cards loaded");
            ShowAllCardInfoLoaded();

            List<DataCreatorBase> ReadData()
            {
                StreamReader reader = new(FullPathLoaded);
                string jsonData = reader.ReadToEnd();
                reader.Close();
                Type listOfType = typeof(List<>).MakeGenericType(structDataLoaded.GetType());
                IEnumerable<DataCreatorBase> allDataAsStructData = JsonConvert.DeserializeObject(jsonData, listOfType) as IEnumerable<DataCreatorBase>;
                return allDataAsStructData.ToList();
            }
        }

        /*******************************************************************/
        [ShowIfGroup("IsJSONLoaded")]
        [BoxGroup("IsJSONLoaded/List", ShowLabel = false)]
        [HorizontalGroup("IsJSONLoaded/List/FileList", LabelWidth = 50, MarginRight = 0.1f)]
        [OnValueChanged("Search")]
        [InlineProperty(LabelWidth = 130)]
        [SerializeField] private string find = string.Empty;
        private void Search()
        {
            cardsHead.Clear();
            allCardData.FindAll(cardInfo => cardInfo.Contains(find)).ForEach(cardInfo => cardsHead.Add(new Header(cardInfo, SelecCard, DeleteCard)));
        }

        [BoxGroup("IsJSONLoaded/List")]
        [HorizontalGroup("IsJSONLoaded/List/FileList", Width = 50)]
        [Button(SdfIconType.ExclamationOctagon, Name = "")]
        private void CreateRow()
        {
            DataCreatorBase newCard = Activator.CreateInstance(structDataLoaded.GetType()) as DataCreatorBase;
            allCardData.Add(newCard);
            ShowAllCardInfoLoaded();
            cardSelected = newCard;
        }

        [BoxGroup("IsJSONLoaded/List")]
        [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100, IsReadOnly = true, HideToolbar = true, AlwaysExpanded = true)]
        [SerializeField] private List<Header> cardsHead = new();

        /*******************************************************************/
        [ShowIfGroup("IsCardSelected")]
        [BoxGroup("IsCardSelected/Editor", LabelText = "Card", CenterLabel = true)]
        [HideLabel]
        [HideReferenceObjectPicker]
        [SerializeField] private DataCreatorBase cardSelected;

        /*******************************************************************/
        [BoxGroup("IsCardSelected/Save", ShowLabel = false)]
        [HorizontalGroup("IsCardSelected/Save/Buttons", MarginLeft = 50)]
        [Button(Name = "Save")]
        private void Save()
        {
            string serializeInfo = JsonConvert.SerializeObject(allCardData, Formatting.Indented);

            StreamWriter writer = new(FullPathLoaded);
            writer.WriteLine(serializeInfo);
            writer.Close();
            AssetDatabase.Refresh();
            cardSelected = null;
            ShowAllCardInfoLoaded();
        }

        [BoxGroup("IsCardSelected/Save")]
        [HorizontalGroup("IsCardSelected/Save/Buttons", MarginRight = 50)]
        [Button(Name = "Save as")]
        private void SaveAs()
        {
            string newPath = EditorUtility.SaveFilePanel("Save JSON", DATA_PATH, "New JSON", "json");
            if (string.IsNullOrEmpty(newPath)) return;

            string serializeInfo = JsonConvert.SerializeObject(allCardData, Formatting.Indented);
            StreamWriter writer = new(newPath);
            writer.WriteLine(serializeInfo);
            writer.Close();
            AssetDatabase.Refresh();
            cardSelected = null;
            ShowAllCardInfoLoaded();
        }

        /*******************************************************************/
        private void SelecCard(DataCreatorBase card) => cardSelected = card;

        private void DeleteCard(DataCreatorBase card)
        {
            allCardData.Remove(card);
            ShowAllCardInfoLoaded();
        }

        private void ShowAllCardInfoLoaded()
        {
            cardsHead.Clear();
            allCardData.ForEach(cardInfo => cardsHead.Add(new Header(cardInfo, SelecCard, DeleteCard)));
        }
    }
}
#pragma warning restore IDE0051, IDE0052 // Remove unused private members