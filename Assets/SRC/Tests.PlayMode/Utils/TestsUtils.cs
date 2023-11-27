using UnityEngine;
using TMPro;
using System.Reflection;
using System;

namespace MythsAndHorrors.PlayMode.Tests
{
    public static class TestsUtils
    {
        public static string GetTextFromThis(this Transform parentTransform, string gameObjectName)
        {
            if (parentTransform == null) throw new ArgumentNullException("parentTransform cant be null");
            if (string.IsNullOrEmpty(gameObjectName)) throw new ArgumentNullException("gameObjectName cant be null or empty");

            Transform targetTransform = parentTransform.FindDeepChild(gameObjectName)
                ?? throw new InvalidOperationException($"A GameObject with the name {gameObjectName} was not found among the children of the given Transform.");
            TextMeshPro textMeshPro = targetTransform.GetComponentInChildren<TextMeshPro>()
                ?? throw new InvalidOperationException($"A TextMeshPro component was not found in the GameObject {targetTransform.name}.");
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

            FieldInfo field = null;
            foreach (FieldInfo fieldInfo in objectTarget.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fieldInfo.Name == memberName)
                {
                    field = fieldInfo;
                    break;
                }
            }

            if (field == null) throw new ArgumentException($"No private field named {memberName} found in {objectTarget.GetType().Name}.");
            if (field.FieldType != typeof(T)) throw new InvalidOperationException($"Field {memberName} is not of type {typeof(T).Name}.");
            return (T)field.GetValue(objectTarget);
        }
    }
}
