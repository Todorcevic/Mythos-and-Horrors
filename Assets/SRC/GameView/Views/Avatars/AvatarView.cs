using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class AvatarView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _glow;
        [SerializeField, Required, ChildGameObjectsOnly] private StatUIView _healthStat;
        [SerializeField, Required, ChildGameObjectsOnly] private StatUIView _sanityStat;
        [SerializeField, Required, ChildGameObjectsOnly] private StatUIView _resourcesStat;
        [SerializeField, Required, ChildGameObjectsOnly] private StatUIView _hintsStat;
        [SerializeField, Required, ChildGameObjectsOnly] private ActionController _actionController;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        public bool IsVoid => Investigator == null;
        public Investigator Investigator { get; private set; }
        public Sprite Image => _picture.sprite;

        /*******************************************************************/
        public void Init(Investigator investigator)
        {
            Investigator = investigator;
            SetPicture();
            SetStats();
            gameObject.SetActive(true);
        }

        public Tween Select() => transform.DOScale(1.25f, ViewValues.FAST_TIME_ANIMATION);

        public Tween Deselect() => transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);

        public Tween ActivateGlow() => _glow.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);

        public Tween DeactivateGlow() => _glow.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);

        public Tween Show() => _picture.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        public Tween Hide() => _picture.DOFade(ViewValues.DEFAULT_FADE, ViewValues.FAST_TIME_ANIMATION);

        private async void SetPicture() => await _picture.LoadCardSprite(Investigator.InvestigatorCard.Info.Code);

        private void SetStats()
        {
            _healthStat.SetStat(Investigator.Health);
            _sanityStat.SetStat(Investigator.Sanity);
            _resourcesStat.SetStat(Investigator.Resources);
            _hintsStat.SetStat(Investigator.Keys);
            _actionController.Init(Investigator.CurrentActions, Investigator.MaxActions);
        }

        /*******************************************************************/
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterAnimation();
            _cardViewsManager.GetAvatarCardView(Investigator).CardSensor.MouseEnter();
        }

        public void PointerEnterAnimation()
        {
            _picture.DOColor(new Color(0.8f, 0.8f, 0.8f, _picture.color.a), ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            PointerExitAnimation();
            _cardViewsManager.GetAvatarCardView(Investigator).CardSensor.MouseExit();
        }

        public void PointerExitAnimation()
        {
            _picture.DOColor(new Color(1f, 1f, 1f, _picture.color.a), ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
        }

        async void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            await _ioActivatorComponent.DeactivateCardSensors();
            await _swapInvestigatorPresenter.Select(Investigator).AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
        }
    }
}
