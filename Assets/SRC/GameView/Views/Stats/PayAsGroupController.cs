using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class PayAsGroupController : MonoBehaviour
    {
        public int CurrentValue { get; private set; }

        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private ButtonAsGroup _buttonUp;
        [SerializeField, Required, ChildGameObjectsOnly] private ButtonAsGroup _buttonDown;

        /*******************************************************************/
        public void Init()
        {
            _buttonUp.OnClick += () => ChangeValue(1);
            _buttonDown.OnClick += () => ChangeValue(-1);
        }

        /*******************************************************************/
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ChangeValue(int increment)
        {
            CurrentValue += increment;
            _value.text = CurrentValue.ToString();
        }
    }
}

