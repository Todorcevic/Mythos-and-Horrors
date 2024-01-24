using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class EffectView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _avatarLeft;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _avatarRight;

        private float? _avatarLeftWidth;
        private float? _avatarRightWidth;

        private float AvatarLeftWidth => _avatarLeftWidth ??= -_avatarLeft.GetComponent<RectTransform>().rect.width;
        private float AvatarRightWidth => _avatarRightWidth ??= -_avatarRight.GetComponent<RectTransform>().rect.width;
        public bool IsEmpty => !gameObject.activeSelf;

        /*******************************************************************/
        public void SetDescription(string text)
        {
            gameObject.SetActive(text != string.Empty);
            _description.text = text;
        }

        public void SetAvatarLeft(Sprite sprite)
        {
            _description.margin += sprite == null ? new Vector4(AvatarLeftWidth, 0, 0, 0) : Vector4.zero;
            _avatarLeft.enabled = sprite != null;
            _avatarLeft.sprite = sprite;
        }

        public void SetAvatarRight(Sprite sprite)
        {
            _description.margin += sprite == null ? new Vector4(0, 0, AvatarRightWidth, 0) : Vector4.zero;
            _avatarRight.enabled = sprite != null;
            _avatarRight.sprite = sprite;
        }

        public void Clear()
        {
            SetDescription(string.Empty);
            _description.margin = Vector4.zero;
        }
    }
}
