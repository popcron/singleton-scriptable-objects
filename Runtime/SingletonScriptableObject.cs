#nullable enable
using SingletonScriptableObjects;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnityEngine
{
    /// <summary>
    /// Base type for singleton <see cref="ScriptableObject"/> assets that can be
    /// accessed through the <see cref="Singleton"/> property.
    /// </summary>
    [ExecuteAlways]
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T? singleton;

        /// <summary>
        /// References the singleton instance <typeparamref name="T"/>.
        /// </summary>
        public static T Singleton
        {
            get
            {
                if (singleton == null)
                {
#if UNITY_EDITOR
                    if (TryFindSingleton(out T? foundSingleton))
                    {
                        singleton = foundSingleton;
                    }
                    else
                    {
                        throw new NullReferenceException($"Singleton of type `{typeof(T)}` does not exist in project");
                    }
#else
                    throw new InvalidOperationException($"Singleton of type `{typeof(T)}` does not exist");
#endif
                }

                return singleton;
            }
        }

        protected virtual void OnEnable()
        {
            if (singleton is null)
            {
                singleton = (T)this;
                SingletonScriptableObjects.SingletonScriptableObjects.Register(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (singleton is not null)
            {
                SingletonScriptableObjects.SingletonScriptableObjects.Unregister(this);
                singleton = null;
            }
        }

#if UNITY_EDITOR
        private static bool TryFindSingleton([NotNullWhen(true)] out T? foundSingleton)
        {
            if (PreloadedAssets.TryGet(out T? preloadedSingleton))
            {
                foundSingleton = preloadedSingleton;
                return true;
            }

            string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).FullName}");
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                T? asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    foundSingleton = asset;
                    return true;
                }
            }

            foundSingleton = null;
            return false;
        }
#endif
    }
}