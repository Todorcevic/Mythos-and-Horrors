using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MythsAndHorrors.GameView
{
    public class ImageLoaderUseCase
    {
        public Task<Sprite> LoadSpriteAsync(string key)
        {

            return Addressables.LoadAssetAsync<Sprite>(key).Task;


        }
    }
}
