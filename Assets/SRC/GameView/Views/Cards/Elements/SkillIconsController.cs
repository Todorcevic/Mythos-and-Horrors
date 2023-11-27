using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
{
    public class SkillIconsController : MonoBehaviour
    {
        private const float Z_OFFSET = -0.0001f;
        [SerializeField, Required, AssetsOnly] private SkillIconView _skillIconPrefab;
        private int totalIcons;

        /*******************************************************************/
        public void SetSkillIconView(int amount, Sprite icon, Sprite holder)
        {
            for (int i = 0; i < amount; i++)
            {
                _skillIconPrefab.SetSkillIcon(icon, holder);
                SkillIconView skillIconInstantiate = Instantiate(_skillIconPrefab, transform);
                skillIconInstantiate.transform.localPosition += new Vector3(0, 0, totalIcons++ * Z_OFFSET);
            }
        }
    }
}
