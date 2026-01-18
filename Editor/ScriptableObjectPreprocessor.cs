using UnityEditor;
using UnityEngine;

namespace SingletonScriptableObjects
{
    public class ScriptableObjectPreprocessor : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (asset != null && SingletonScriptableObjects.IsSingleton(asset))
            {
                PreloadedAssets.RemoveWithType(asset.GetType());
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}