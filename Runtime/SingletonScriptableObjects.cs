#nullable enable
using System;
using UnityEngine;

namespace SingletonScriptableObjects
{
    public static class SingletonScriptableObjects
    {
        public static Action<ScriptableObject>? onRegister;
        public static Action<ScriptableObject>? onUnregister;
        private static readonly List<ScriptableObject> singletons = new();

        /// <summary>
        /// All singleton <see cref="ScriptableObject"/> instances.
        /// </summary>
        public static IReadOnlyList<ScriptableObject> Singletons => singletons;

        internal static void Register(ScriptableObject scriptableObject)
        {
            if (EnsureIsPreloaded(scriptableObject))
            {
                singletons.Add(scriptableObject);
                onRegister?.Invoke(scriptableObject);   
            }
        }

        internal static void Unregister(ScriptableObject scriptableObject)
        {
            if (singletons.Remove(scriptableObject))
            {
                onUnregister?.Invoke(scriptableObject);
            }
        }

        internal static bool EnsureIsPreloaded(ScriptableObject scriptableObject)
        {
            if (!PreloadedAssets.Contains(scriptableObject))
            {
                if (PreloadedAssets.ContainsWithType(scriptableObject.GetType()))
                {
                    Debug.LogError($"A different instance of the singleton ScriptableObject of type {scriptableObject.GetType().FullName} is already registered as a preloaded asset. Only one instance of a singleton ScriptableObject type can be registered at a time.", scriptableObject);
                    return false;
                }

                PreloadedAssets.RemoveWithType(scriptableObject.GetType());
                PreloadedAssets.Add(scriptableObject);
            }

            return true;
        }

        /// <summary>
        /// Checks if the given <paramref name="scriptableObject"/> is a singleton ScriptableObject.
        /// </summary>
        public static bool IsSingleton(ScriptableObject scriptableObject)
        {
            Type currentType = scriptableObject.GetType();
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