using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.UI;

namespace MythsAndHorrors.GameView
{
    public static class ImageExtension
    {
        private const string FAIL_IMAGE = "Fails/Fail.png";
        private static IResourceLocator _resourceLocator;

        /*******************************************************************/
        public static async void LoadCardSprite(this SpriteRenderer imagen, string address) =>
            imagen.sprite = await LoadSpriteAsync("Cards/" + address + ".png") ?? await LoadFailImage();

        public static async void LoadCardSprite(this Image imagen, string address) =>
            imagen.sprite = await LoadSpriteAsync("Cards/" + address + ".png") ?? await LoadFailImage();

        public static async void LoadHistorySprite(this Image imagen, string address) =>
         imagen.sprite = await LoadSpriteAsync("Screens/" + address + ".jpg") ?? await LoadFailImage();

        private async static Task<Sprite> LoadSpriteAsync(string key)
        {
            _resourceLocator ??= await Addressables.InitializeAsync().Task;
            if (!_resourceLocator.Keys.Contains(key)) return null;
            return await Addressables.LoadAssetAsync<Sprite>(key).Task;
        }

        private static async Task<Sprite> LoadFailImage() => await Addressables.LoadAssetAsync<Sprite>(FAIL_IMAGE).Task;
    }
}
