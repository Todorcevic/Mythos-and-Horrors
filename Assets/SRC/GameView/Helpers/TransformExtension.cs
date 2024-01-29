using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public static class TransformExtension
    {
        public static void ResetToZero(this Transform transform)
        {
            transform.localPosition = transform.localScale = transform.localEulerAngles = Vector3.zero;
        }

        public static void CloneFrom(this Transform transform, Transform origin)
        {
            transform.position = origin.position;
            transform.rotation = origin.rotation;
            transform.localScale = origin.localScale;
        }

        public static void ChangeAllLayers(this Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            foreach (Transform child in transform)
            {
                child.ChangeAllLayers(layer);
            }
        }
    }
}
