using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AvatarView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _glow;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _hints;
        [SerializeField, Required, ChildGameObjectsOnly] private TurnController _turnController;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        public bool IsVoid => Investigator == null;
        public Investigator Investigator { get; private set; }
        public Sprite Image => _picture.sprite;

        /*******************************************************************/
        public void Init(Investigator investigator)
        {
            Investigator = investigator;
            SetPicture();
            SetHealth(investigator.InvestigatorCard.Info.Health ?? 0);
            SetSanity(investigator.InvestigatorCard.Info.Sanity ?? 0);
            SetHints(investigator.InvestigatorCard.Info.Hints ?? 0);
            gameObject.SetActive(true);
        }

        public Tween Select() => transform.DOScale(1.25f, ViewValues.FAST_TIME_ANIMATION);

        public Tween Deselect() => transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);

        public Tween ActivateGlow() => _glow.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);

        public Tween DeactivateGlow() => _glow.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);

        public void SetHealth(int amount) => _health.text = amount.ToString();

        public void SetSanity(int amount) => _sanity.text = amount.ToString();

        public void SetHints(int amount) => _hints.text = amount.ToString();

        public void ShowTurns(int amount) => _turnController.TurnOn(amount);

        private async void SetPicture() => await _picture.LoadAvatarSprite(Investigator.InvestigatorCard.Info.Code);

        /*******************************************************************/
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _picture.DOColor(new Color(0.8f, 0.8f, 0.8f), ViewValues.FAST_TIME_ANIMATION);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _picture.DOColor(Color.white, ViewValues.FAST_TIME_ANIMATION);
        }

        async void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _ioActivatorComponent.DeactivateCardSensors();
            await _swapInvestigatorPresenter.Select(Investigator).AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
        }
    }
}
