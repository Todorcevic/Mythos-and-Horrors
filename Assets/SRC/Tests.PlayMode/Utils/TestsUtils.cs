﻿using UnityEngine;
using TMPro;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace MythosAndHorrors.PlayMode.Tests
{
    public static class TestsUtils
    {
        public static string GetTextFromThis(this Transform parentTransform, string gameObjectName)
        {
            if (parentTransform == null) throw new ArgumentNullException("parentTransform cant be null");
            if (string.IsNullOrEmpty(gameObjectName)) throw new ArgumentNullException("gameObjectName cant be null or empty");

            Transform targetTransform = parentTransform.FindDeepChild(gameObjectName) ??
                 throw new InvalidOperationException($"A GameObject with the name {gameObjectName} was not found among the children of the given Transform.");
            TextMeshPro textMeshPro = targetTransform.GetComponentInChildren<TextMeshPro>(includeInactive: true) ??
                 throw new InvalidOperationException($"A TextMeshPro component was not found in the GameObject {targetTransform.name}.");
            return textMeshPro.text;
        }

        public static Transform FindDeepChild(this Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name) return child;
                Transform result = child.FindDeepChild(name);
                if (result != null) return result;
            }
            return null;
        }

        public static void ExecutePrivateMethod(this object objectTarget, string methodName, params object[] parameters)
        {
            if (objectTarget == null) throw new ArgumentNullException("objectTarget cant be null");
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName cant be null or empty");

            Type objectType = objectTarget.GetType();
            MethodInfo method;

            while (objectType != null)
            {
                method = objectType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(objectTarget, parameters);
                    return;
                }

                objectType = objectType.BaseType;
            }

            throw new ArgumentNullException("Private method not found: " + objectTarget.GetType().Name + " - " + methodName);
        }

        public static T GetPrivateMember<T>(this object objectTarget, string memberName)
        {
            if (objectTarget == null) throw new ArgumentNullException("objectTarget cant be null");
            if (string.IsNullOrEmpty(memberName)) throw new ArgumentNullException("memberName cant be null or empty");

            Type objectType = objectTarget.GetType();
            FieldInfo field;
            PropertyInfo property;

            while (objectType != null)
            {
                field = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                  .FirstOrDefault(fieldInfo => fieldInfo.Name == memberName && fieldInfo.FieldType == typeof(T));
                if (field != null) return (T)field.GetValue(objectTarget);

                property = objectType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                               .FirstOrDefault(propertyInfo => propertyInfo.Name == memberName && propertyInfo.PropertyType == typeof(T));
                if (property != null) return (T)property.GetValue(objectTarget);

                objectType = objectType.BaseType;
            }

            throw new ArgumentNullException("Private member not found: " + objectTarget.GetType().Name + " - " + memberName);
        }

        public static IEnumerator AsCoroutine(this Task task)
        {
            while (!task.IsCompleted) yield return null;
            task.GetAwaiter().GetResult();
        }

        private class VoidMonoBehaviour : MonoBehaviour { }
        private static VoidMonoBehaviour coroutineCreator;
        public static Task AsTask(this IEnumerator coroutine)
        {
            TaskCompletionSource<bool> taskCompletionSource = new();
            if (coroutineCreator == null) coroutineCreator = new GameObject(nameof(VoidMonoBehaviour)).AddComponent<VoidMonoBehaviour>();
            coroutineCreator.StartCoroutine(RunCoroutine(coroutine, taskCompletionSource));
            return taskCompletionSource.Task;

            static IEnumerator RunCoroutine(IEnumerator coroutine, TaskCompletionSource<bool> taskCompletionSource)
            {
                while (coroutine.MoveNext()) yield return coroutine.Current;
                taskCompletionSource.SetResult(true);
            }
        }

        public static IEnumerator Fast(this IEnumerator coroutine)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return coroutine;
            Time.timeScale = currentTimeScale;
        }
    }
}
