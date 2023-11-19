using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class SkillIconsController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private SkillIconView _skillIconPrefab;

        /*******************************************************************/
        public void SetSkillIconView(int amount, Sprite icon, Sprite holder)
        {
            for (int i = 0; i < amount; i++)
            {
                _skillIconPrefab.SetSkillIcon(icon, holder);
                Instantiate(_skillIconPrefab, transform);
            }
        }
    }
}
