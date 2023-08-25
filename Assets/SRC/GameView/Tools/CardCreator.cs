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

        /*******************************************************************/
        [BoxGroup("Struct Data")]
        [TypeFilter("GetCreatorDataTypes")]
        [SerializeField, LabelText("Load Struct Data")]
        private DataCreatorBase structDataLoaded;

        private IEnumerable<Type> GetCreatorDataTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract
                    && !type.IsGenericTypeDefinition
                    && typeof(DataCreatorBase).IsAssignableFrom(type));
        }

        /*******************************************************************/
        [ShowIfGroup("IsStructDataLoaded")]
        [BoxGroup("IsStructDataLoaded/JSON File")]
        [HorizontalGroup("IsStructDataLoaded/JSON File/FileGroup", LabelWidth = 100)]
        [InfoBox("$newFileNameinfoBoxMessage", InfoMessageType.Warning, "@ !string.IsNullOrEmpty(newFileNameinfoBoxMessage)")]
        [SerializeField, LabelText("New JSON file")]
        private string newJSONFileName;

        [BoxGroup("IsStructDataLoaded/JSON File")]
        [HorizontalGroup("IsStructDataLoaded/JSON File/FileGroup", Width = 16, LabelWidth = 100)]
        [Button("+")]
        private void CreateFile()
        {
            if (string.IsNullOrEmpty(newJSONFileName))
            {
                newFileNameinfoBoxMessage = "File name cant be empty!";
                return;
            }

            JSONFileLoaded = Path.Combine(DATA_PATH, newJSONFileName + ".json");
            if (File.Exists(JSONFileLoaded))
            {
                newFileNameinfoBoxMessage = "File exist!";
                return;
            }

            File.WriteAllText(JSONFileLoaded, "Contenido del archivo.");
            structDataLoaded.IsEditable = true;
            newFileNameinfoBoxMessage = string.Empty;
        }

        /*******************************************************************/
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
                IEnumerable<object> allDataAsStructData = JsonConvert.DeserializeObject(jsonData, listOfType) as IEnumerable<object>;
                return allDataAsStructData.Cast<DataCreatorBase>().ToList();
            }

            void ShowAllCardInfoLoaded()
            {
                cardsHead = new List<Header>();
                allCardData.ForEach(cardInfo => cardsHead.Add(new Header(cardInfo, SelecCard)));
            }
        }

        private void SelecCard(DataCreatorBase card)
        {
            cardSelected = card;
            //imageCard = await LoadTexture();

            //async Task<Texture2D> LoadTexture()
            //{
            //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(IMAGE_URL + card.Image);
            //    request.SendWebRequest();

            //    while (!request.isDone) await Task.Yield();

            //    if (request.result != UnityWebRequest.Result.Success)
            //    {
            //        Debug.Log(request.error);
            //        return null;
            //    }

            //    return DownloadHandlerTexture.GetContent(request);
            //}
        }

        /*******************************************************************/
        [BoxGroup("List")]
        [OnValueChanged("Search")]
        [InlineProperty(LabelWidth = 130)]
        [SerializeField] private string find = string.Empty;

        private void Search()
        {
            cardsHead = new List<Header>();
            allCardData.FindAll(cardInfo => cardInfo.Contains(find)).ForEach(cardInfo => cardsHead.Add(new Header(cardInfo, SelecCard)));
        }

        [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100, IsReadOnly = true, HideToolbar = true)]
        [SerializeField] private List<Header> cardsHead;

        /*******************************************************************/
        [Space(10)]
        [HideLabel]
        [SerializeField] private DataCreatorBase cardSelected;

        /*******************************************************************/
        private string FullPathLoaded => DATA_PATH + JSONFileLoaded;
        private bool IsStructDataLoaded => structDataLoaded != null;
        private bool IsFileNameEmpty => string.IsNullOrEmpty(newJSONFileName);

        /*******************************************************************/
        [MenuItem("Tools/Cards/Card Editor")]
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



    }
}
#pragma warning restore IDE0051, IDE0052 // Remove unused private members