#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;
using System.Diagnostics.CodeAnalysis;

[assembly: InternalsVisibleTo("SingletonScriptableObjects.Editor")]
namespace SingletonScriptableObjects
{
    internal static class PreloadedAssets
    {
#if UNITY_EDITOR
        public static void RemoveWithType(Type type)
        {
            bool removedAny = false;
            List<Object> preloadedAssets = new(UnityEditor.PlayerSettings.GetPreloadedAssets());
            for (int i = preloadedAssets.Count - 1; i >= 0; i--)
            {
                Object obj = preloadedAssets[i];
                if (obj != null && obj.GetType() == type)
                {
                    preloadedAssets.Remove(obj);
                    removedAny = true;
                }
            }

            if (removedAny)
            {
                UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            }
        }

        public static bool ContainsWithType(Type type)
        {
            Object[] preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets();
            for (int i = 0; i < preloadedAssets.Length; i++)
            {
                Object obj = preloadedAssets[i];
                if (obj != null && obj.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains(Object asset)
        {
            Object[] preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets();
            foreach (Object preloadedAsset in preloadedAssets)
            {
                if (preloadedAsset == asset)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Add(Object asset)
        {
            Object[] preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets();
            Array.Resize(ref preloadedAssets, preloadedAssets.Length + 1);
            preloadedAssets[preloadedAssets.Length - 1] = asset;
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets);
        }

        public static bool TryGet<T>([NotNullWhen(true)] out T? foundAsset) where T : Object
        {
            Object[] preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets();
            foreach (Object preloadedAsset in preloadedAssets)
            {
                if (preloadedAsset != null && preloadedAsset is T typedAsset)
                {
                    foundAsset = typedAsset;
                    return true;
                }
            }

            foundAsset = null;
            return false;
        }
#else
        public static void RemoveWithType(Type type) { }
        public static bool ContainsWithType(Type type) => false;
        public static bool Contains(Object asset) => false;
        public static void Add(Object asset) { }
#endif
    }
}