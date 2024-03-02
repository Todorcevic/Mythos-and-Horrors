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
            SetStats();
            gameObject.SetActive(true);
        }

        public Tween Select() => transform.DOScale(1.25f, ViewValues.FAST_TIME_ANIMATION);

        public Tween Deselect() => transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);

        public Tween ActivateGlow() => _glow.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);

        public Tween DeactivateGlow() => _glow.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);

        private async void SetPicture() => await _picture.LoadAvatarSprite(Investigator.InvestigatorCard.Info.Code);

        private void SetStats()
        {
            _healthStat.SetStat(Investigator.Health);
            _sanityStat.SetStat(Investigator.Sanity);
            _resourcesStat.SetStat(Investigator.Resources);
            _hintsStat.SetStat(Investigator.Hints);
            _turnController.Init(Investigator.Turns);
        }

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
            await _ioActivatorComponent.DeactivateCardSensors();
            await _swapInvestigatorPresenter.Select(Investigator).AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
        }
    }
}
