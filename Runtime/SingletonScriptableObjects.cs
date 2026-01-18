#nullable enable
using System;
using UnityEngine;

namespace SingletonScriptableObjects
{
    internal static class SingletonScriptableObjects
    {
        public static void Register(ScriptableObject singletonScriptableObject)
        {
            EnsureIsPreloaded(singletonScriptableObject);
        }

        public static void Unregister(ScriptableObject singletonScriptableObject)
        {
        }

        public static void EnsureIsPreloaded(ScriptableObject singletonScriptableObject)
        {
            if (!PreloadedAssets.Contains(singletonScriptableObject))
            {
                if (PreloadedAssets.ContainsWithType(singletonScriptableObject.GetType()))
                {
                    Debug.LogError($"A different instance of the singleton ScriptableObject of type {singletonScriptableObject.GetType().FullName} is already registered as a preloaded asset. Only one instance of a singleton ScriptableObject type can be registered at a time.", singletonScriptableObject);
                    return;
                }

                PreloadedAssets.RemoveWithType(singletonScriptableObject.GetType());
                PreloadedAssets.Add(singletonScriptableObject);
            }
        }

        public static bool IsSingleton(ScriptableObject singletonScriptableObject)
        {
            Type currentType = singletonScriptableObject.GetType();
            while (currentType != typeof(ScriptableObject))
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(SingletonScriptableObject<>))
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }
    }
}