#pragma warning disable IDE0051, IDE0052 // Remove unused private members
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using FilePathAttribute = Sirenix.OdinInspector.FilePathAttribute;

namespace MythsAndHorrors.Tools
{
    public class CardCreator : OdinEditorWindow
    {
        private const string DATA_PATH = "Assets/Data/";
        private const string FILE_EXTENSION = ".json";
        private readonly JsonSerializerSettings jsonSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore
        };

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
        [HideIf("IsStructDataLoaded")]
        [BoxGroup("Struct Data", ShowLabel = false)]
        [TypeFilter("GetCreatorDataTypes")]
        [OnValueChanged("OnStructDataLoadedChanged")]
        [SerializeField, LabelText("Load Struct Data")]
        private DataCreatorBase structDataLoaded;

        private IEnumerable<Type> GetCreatorDataTypes() => Assembly.GetExecutingAssembly()
            .GetTypes().Where(type => type.BaseType == typeof(DataCreatorBase));

        private void OnStructDataLoadedChanged()
        {
            if (structDataLoaded == null) return;

            structDataType = structDataLoaded.GetType();
            JSONFileLoaded = string.Empty;
            cardSelected = null;
            newFileNameinfoBoxMessage = string.Empty;
            //typeSelected = GetCardTypes().FirstOrDefault();
        }

        [ShowIfGroup("IsStructDataLoaded")]
        [BoxGroup("IsStructDataLoaded/Struct Data", ShowLabel = false)]
        [ReadOnly, SerializeField, LabelText("Struct Loaded")]
        private Type structDataType;

        /*******************************************************************/
        [ShowIfGroup("IsStructDataLoaded")]
        [BoxGroup("IsStructDataLoaded/JSON File", ShowLabel = false)]
        [HorizontalGroup("IsStructDataLoaded/JSON File/FileGroup", LabelWidth = 100)]
        [InfoBox("$newFileNameinfoBoxMessage", InfoMessageType.Warning, "@ !string.IsNullOrEmpty(newFileNameinfoBoxMessage)")]
        [OnInspectorInit("InitializeNewFileName")]
        [SerializeField, LabelText("New JSON file")]
        private string newJSONFileName;

        private void InitializeNewFileName()
        {
            int suffix = 0;
            string fileName = structDataLoaded.GetType().Name;
            while (File.Exists(DATA_PATH + fileName + FILE_EXTENSION))
            {
                suffix++;
                fileName = structDataLoaded.GetType().Name + suffix;
            }

            newJSONFileName = fileName;
        }

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

            JSONFileLoaded = newJSONFileName + FILE_EXTENSION;
            if (File.Exists(FullPathLoaded))
            {
                newFileNameinfoBoxMessage = "File exist!";
                return;
            }

            File.WriteAllText(FullPathLoaded, string.Empty);
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
                return (JsonConvert.DeserializeObject(jsonData, listOfType, jsonSettings) as IEnumerable<DataCreatorBase>).ToList();
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

        //[BoxGroup("IsJSONLoaded/List", ShowLabel = false)]
        //[HorizontalGroup("IsJSONLoaded/List/FileList")]
        //[ValueDropdown("GetCardTypes"), LabelText("Type")]
        //public Type typeSelected;

        //private IEnumerable<Type> GetCardTypes() => Assembly.GetExecutingAssembly()
        //     .GetTypes().Where(type => type.BaseType != typeof(DataCreatorBase)
        //         && type != typeof(DataCreatorBase)
        //         && typeof(DataCreatorBase).IsAssignableFrom(type));

        [BoxGroup("IsJSONLoaded/List")]
        [HorizontalGroup("IsJSONLoaded/List/FileList", Width = 50)]
        [GUIColor("@Color.green")]
        [Button(SdfIconType.Plus, Name = "")]
        private void CreateRow()
        {
            DataCreatorBase newCard = Activator.CreateInstance(structDataType) as DataCreatorBase;
            allCardData.Add(newCard);
            ShowAllCardInfoLoaded();
            cardSelected = newCard;
        }

        /*******************************************************************/
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
        [HorizontalGroup("IsCardSelected/Save/Buttons")]
        [GUIColor("@Color.green")]
        [Button(ButtonSizes.Large, Name = "Save")]
        private void Save() => Save(FullPathLoaded);

        [ShowIf("IsJSONLoaded")]
        [BoxGroup("SaveAs", showLabel: false)]
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
            string serializeInfo = JsonConvert.SerializeObject(allCardData, Formatting.Indented, jsonSettings);
            StreamWriter writer = new(path);
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