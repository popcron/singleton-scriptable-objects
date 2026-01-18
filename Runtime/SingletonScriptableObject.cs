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
        public static bool TryFindSingleton([NotNullWhen(true)] out T? foundSingleton)
        {
            if (PreloadedAssets.TryGet(out T? preloadedSingleton))
            {
                foundSingleton = preloadedSingleton;
                return true;
            }

            GUID[] guids = UnityEditor.AssetDatabase.FindAssetGUIDs($"t:{typeof(T).FullName}");
            foreach (GUID guid in guids)
            {
                T? asset = UnityEditor.AssetDatabase.LoadAssetByGUID<T>(guid);
                if (asset != null)
                {
                    foundSingleton = asset;
                    return true;
                }
            }

            foundSingleton = null;
            return false;
        }
#else
        [Obsolete("TryFindSingleton is only available in the Unity Editor")]
        public static bool TryFindSingleton([NotNullWhen(true)] out T? foundSingleton)
        {
            foundSingleton = singleton;
            return foundSingleton != null;
        }
#endif
    }
}