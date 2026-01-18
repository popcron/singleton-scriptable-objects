using UnityEditor;
using UnityEngine;

namespace SingletonScriptableObjects
{
    public class ScriptableObjectPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string path in importedAssets)
            {
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset != null && SingletonScriptableObjects.IsSingleton(asset))
                {
                    SingletonScriptableObjects.EnsureIsPreloaded(asset);
                }
            }
        }
    }
}