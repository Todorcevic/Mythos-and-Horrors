using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public static class ImageExtension
    {
        private const string FAIL_IMAGE = "Fails/Fail.png";
        private static readonly List<AsyncOperationHandle<Sprite>> handles = new();
        private static IResourceLocator _resourceLocator;
        private static Sprite _failImage;

        private static Sprite FailImage => _failImage ??= Addressables.LoadAssetAsync<Sprite>(FAIL_IMAGE).WaitForCompletion();

        /*******************************************************************/
        public static async Task LoadCardSprite(this SpriteRenderer imagen, string address)
        {
            Sprite cardSprite = await LoadSpriteAsync("Cards/" + address + ".png") ?? FailImage;
            if (imagen != null) imagen.sprite = cardSprite;
        }

        public static async Task LoadAvatarSprite(this Image imagen, string address)
        {
            Sprite avatarSprite = await LoadSpriteAsync("Cards/" + address + ".png") ?? FailImage;
            if (imagen != null) imagen.sprite = avatarSprite;
        }

        public static async Task LoadHistorySprite(this Image imagen, string address)
        {
            Sprite historySprite = await LoadSpriteAsync("Screens/" + address + ".png") ?? FailImage;
            if (imagen != null) imagen.sprite = historySprite;
        }

        public static bool IsAllDone()
        {
            foreach (AsyncOperationHandle<Sprite> handle in handles)
                if (!handle.IsDone) return false;
            handles.Clear();
            return true;
        }

        private async static Task<Sprite> LoadSpriteAsync(string key)
        {
            _resourceLocator ??= await Addressables.InitializeAsync().Task;
            if (!_resourceLocator.Keys.Contains(key)) return null;

            AsyncOperationHandle<Sprite> operationHandle = Addressables.LoadAssetAsync<Sprite>(key);
            handles.Add(operationHandle);
            return await operationHandle.Task;
        }
    }
}
