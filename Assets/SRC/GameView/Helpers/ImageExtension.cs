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
        private const string FAIL_IMAGE = "Fails/00000.png";
        private static IResourceLocator _resourceLocator;

        public static async void LoadCardSprite(this SpriteRenderer imagen, string address) => imagen.sprite = await LoadSpriteAsync("Cards/" + address + ".png");

        public static async void LoadCardSprite(this Image imagen, string address) => imagen.sprite = await LoadSpriteAsync("Cards/" + address + ".png");

        private async static Task<Sprite> LoadSpriteAsync(string key)
        {
            _resourceLocator ??= await Addressables.InitializeAsync().Task;

            if (!_resourceLocator.Keys.Contains(key))
            {
                key = FAIL_IMAGE;
                Debug.LogWarning($"Sprite {key} not found");
            }
            return await Addressables.LoadAssetAsync<Sprite>(key).Task;
        }
    }
}
