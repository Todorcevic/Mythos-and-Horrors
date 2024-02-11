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

        public bool IsEmpty => !gameObject.activeSelf;

        /*******************************************************************/
        public void SetDescription(string text)
        {
            gameObject.SetActive(text != string.Empty);
            _description.text = text;
        }

        public void SetAvatarLeft(Sprite sprite)
        {
            _avatarLeft.enabled = sprite != null;
            _avatarLeft.sprite = sprite;
        }

        public void SetAvatarRight(Sprite sprite)
        {
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
