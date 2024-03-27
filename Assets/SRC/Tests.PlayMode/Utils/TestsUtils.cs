using UnityEngine;
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

        public static T GetPrivateMember<T>(this object objectTarget, string memberName)
        {
            if (objectTarget == null) throw new ArgumentNullException("objectTarget cant be null");
            if (string.IsNullOrEmpty(memberName)) throw new ArgumentNullException("memberName cant be null or empty");

            Type objectType = objectTarget.GetType();
            FieldInfo field = null;

            while (objectType != null && field == null)
            {
                field = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                  .FirstOrDefault(fieldInfo => fieldInfo.Name == memberName && fieldInfo.FieldType == typeof(T));

                objectType = objectType.BaseType;
            }

            return (T)field.GetValue(objectTarget);
        }

        public static IEnumerator AsCoroutine(this Task task)
        {
            while (!task.IsCompleted) yield return null;
            task.GetAwaiter().GetResult();
        }

        private static JustForCoroutine coroutineCreator;
        public static Task AsTask(this IEnumerator coroutine)
        {
            TaskCompletionSource<bool> taskCompletionSource = new();
            (coroutineCreator ??= new GameObject(nameof(JustForCoroutine)).AddComponent<JustForCoroutine>()).StartCoroutine(RunCoroutine(coroutine, taskCompletionSource));
            return taskCompletionSource.Task;

            static IEnumerator RunCoroutine(IEnumerator coroutine, TaskCompletionSource<bool> taskCompletionSource)
            {
                while (coroutine.MoveNext()) yield return coroutine.Current;
                taskCompletionSource.SetResult(true);
            }
        }

        private class JustForCoroutine : MonoBehaviour { }
    }
}
