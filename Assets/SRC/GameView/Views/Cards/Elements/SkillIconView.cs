using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
{
    public class SkillIconView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _skillHolder;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _skillIcon;

        public bool IsInactive => !gameObject.activeSelf;

        /*******************************************************************/
        public void SetSkillIcon(Sprite icon, Sprite holder)
        {
            _skillIcon.sprite = icon;
            _skillHolder.sprite = holder != null ? holder : _skillHolder.sprite;
        }
    }
}
