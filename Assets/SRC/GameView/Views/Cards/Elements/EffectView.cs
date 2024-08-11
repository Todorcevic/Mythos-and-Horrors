using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
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

        public void SetPictureLeft(Sprite sprite)
        {
            _avatarLeft.enabled = sprite != null;
            _avatarLeft.sprite = sprite;
        }

        public void SetPictureRight(Sprite sprite)
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
