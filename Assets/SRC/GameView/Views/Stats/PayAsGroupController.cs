using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PayAsGroupController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private ButtonAsGroup _buttonUp;
        [SerializeField, Required, ChildGameObjectsOnly] private ButtonAsGroup _buttonDown;
        [Inject] private readonly PayAsGroupPresenter _payAsGroupPresenter;

        private AvatarCardView _avatarCardView;
        private AvatarCardView AvatarCardView => _avatarCardView ??= GetComponentInParent<AvatarCardView>();

        public int CurrentValue { get; private set; }

        public int InvestigatorAmountHints => AvatarCardView.Investigator.Hints.Value;

        public int InvestigatorHintsLeft => AvatarCardView.Investigator.Hints.Value - CurrentValue;

        /*******************************************************************/
        public void Init()
        {
            _buttonUp.OnClick += () => ChangeValue(1);
            _buttonDown.OnClick += () => ChangeValue(-1);
        }

        /*******************************************************************/
        public void Show()
        {
            Refresh();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ChangeValue(int increment)
        {
            CurrentValue += increment;
            _payAsGroupPresenter.UpdatePayAsGroup(-increment);
            _value.text = CurrentValue.ToString();
        }

        public void Refresh()
        {
            if (CurrentValue <= 0) _buttonDown.gameObject.SetActive(false);
            else _buttonDown.gameObject.SetActive(true);
            if (InvestigatorHintsLeft <= 0) _buttonUp.gameObject.SetActive(false);
            else _buttonUp.gameObject.SetActive(true);
        }
    }
}

