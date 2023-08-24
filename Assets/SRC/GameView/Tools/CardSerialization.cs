#pragma warning disable IDE0051, IDE0052 // Remove unused private members

using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using FilePathAttribute = Sirenix.OdinInspector.FilePathAttribute;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;
using Unity.Plastic.Newtonsoft.Json;
using GameRules;

namespace Tools
{
    public class CardSerialization : OdinEditorWindow
    {
        private const string DATA_PATH = "Assets/Data/";
        private const string SAVE_DATA_PATH = "Assets/Data/New/";
        private const string IMAGE_URL = "https://arkhamdb.com/";

        private List<CardDataModel> allCardInfo;

        private bool IsCardLoaded => cardSelected != null;
        private string FullPathLoaded => DATA_PATH + OriginalJSONPath;

        [MenuItem("Tools/Cards/JSON Editor")]
        private static void Open()
        {
            CardSerialization window = GetWindow<CardSerialization>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 500);
        }

        protected override void Initialize()
        {
            WindowPadding = new Vector4(16, 16, 16, 16);
        }

        [Title("JSON Loaded")]
        [OnValueChanged("LoadData")]
        [FilePath(Extensions = ".json", ParentFolder = DATA_PATH)]
        [SerializeField] private string OriginalJSONPath;

        [Title("List")]
        [OnValueChanged("Search")]
        [InlineProperty(LabelWidth = 130)]
        [SerializeField] private string find = string.Empty;

        [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100, IsReadOnly = true)]
        [SerializeField] private List<Header> cardsHead;

        [HorizontalGroup("Split", 0.75f)]
        [BoxGroup("Split/left")]
        [Space(10)]
        [SerializeField] private CardDataModel cardSelected;

        [BoxGroup("Split/right")]
        [Space(10)]
        [HideLabel]
        [PreviewField(350, ObjectFieldAlignment.Center)]
        public Texture2D imageCard;

        [Button("Open", ButtonSizes.Large)]
        [GUIColor("#9bbbe8")]
        [PropertySpace(10)]
        [BoxGroup("Split/right")]
        [ShowIf("IsCardLoaded")]
        private void LoadURL()
        {
            Application.OpenURL(IMAGE_URL + cardSelected.Image);
        }

        [PropertySpace(20)]
        [GUIColor("#55eb34")]
        [Button("Save", ButtonSizes.Large)]
        [BoxGroup("Split/left")]
        [ShowIf("IsCardLoaded")]
        private void Save()
        {
            string path = SAVE_DATA_PATH + Path.GetFileName(OriginalJSONPath);
            string serializeInfo = JsonConvert.SerializeObject(allCardInfo, Formatting.Indented);
            StreamWriter writer = new(path, true);
            writer.WriteLine(serializeInfo);
            writer.Close();
            AssetDatabase.ImportAsset(path);
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(FullPathLoaded) || !File.Exists(FullPathLoaded)) return;

            allCardInfo = ReadData();
            Debug.Log(allCardInfo.Count + " cards loaded");
            ShowAllCardInfoLoaded();

            List<CardDataModel> ReadData()
            {
                StreamReader reader = new(FullPathLoaded);
                string jsonData = reader.ReadToEnd();
                reader.Close();
                return JsonConvert.DeserializeObject<List<CardDataModel>>(jsonData);
            }

            void ShowAllCardInfoLoaded()
            {
                cardsHead = new List<Header>();
                allCardInfo.ForEach(cardInfo => cardsHead.Add(new Header(cardInfo, SelecCard)));
            }
        }

        private void Search()
        {
            cardsHead = new List<Header>();
            allCardInfo.FindAll(cardInfo => cardInfo.Contains(find)).ForEach(cardInfo => cardsHead.Add(new Header(cardInfo, SelecCard)));
        }

        private async void SelecCard(CardDataModel card)
        {
            cardSelected = card;
            imageCard = await LoadTexture();

            async Task<Texture2D> LoadTexture()
            {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(IMAGE_URL + card.Image);
                request.SendWebRequest();

                while (!request.isDone) await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                    return null;
                }

                return DownloadHandlerTexture.GetContent(request);
            }
        }
    }
}
#pragma warning restore IDE0051, IDE0052 // Remove unused private members