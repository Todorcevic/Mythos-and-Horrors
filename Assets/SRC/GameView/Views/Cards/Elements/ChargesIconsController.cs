using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChargesIconsController : SkillIconsController, IStatable
    {
        private Sprite _icon;
        private Sprite _holder;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void IntialSet(Stat stat, Sprite icon, Sprite holder)
        {
            Stat = stat;
            _icon = icon;
            _holder = holder;
            UpdateValue();
        }

        public Tween UpdateValue()
        {
            ClearAll();
            AddSkillIconView(Stat.Value, _icon, _holder);
            return DOTween.Sequence();
        }

        private void ClearAll()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
