using UnityEditor;

namespace MythosAndHorrors.Tools
{
    public class SpriteExporter : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            if (assetPath.ToLower().Contains("icon"))
            {
                ((TextureImporter)assetImporter).textureType = TextureImporterType.Sprite;
            }
        }
    }
}
