using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class SkillIconView : MonoBehaviour
    {
        [SerializeField, ChildGameObjectsOnly] private SpriteRenderer _skillPlacer;
        [SerializeField, ChildGameObjectsOnly] private SpriteRenderer _skillIcon;

        public bool IsInactive => !gameObject.activeSelf;

        /*******************************************************************/
        public void SetSkillIcon( Sprite icon)
        {
            gameObject.SetActive(true);
            _skillIcon.sprite = icon;
        }
    }
}
