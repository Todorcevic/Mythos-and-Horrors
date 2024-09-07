using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class ActionView : MonoBehaviour
    {
        public bool IsOn { get; private set; }

        [SerializeField, Required, ChildGameObjectsOnly] private Image _image;

        /*******************************************************************/
        public Tween SwitchOn()
        {
            IsOn = true;
            return _image.DOFade(1, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween SwitchOff()
        {
            IsOn = false;
            return _image.DOFade(0, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
