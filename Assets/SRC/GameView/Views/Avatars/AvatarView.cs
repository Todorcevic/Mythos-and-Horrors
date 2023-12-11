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
        [SerializeField, Required, ChildGameObjectsOnly] private Image _selection;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _hints;
        [SerializeField, Required, ChildGameObjectsOnly] private TurnController _turnController;
        [Inject] private readonly SwapAdventurerPresenter _swapAdventurerPresenter;
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;

        public bool IsVoid => Adventurer == null;
        public Adventurer Adventurer { get; private set; }

        /*******************************************************************/
        public void Init(Adventurer adventurer)
        {
            Adventurer = adventurer;
            SetPicture();
            SetHealth(adventurer.AdventurerCard.Info.Health ?? 0);
            SetSanity(adventurer.AdventurerCard.Info.Sanity ?? 0);
            SetHints(adventurer.AdventurerCard.Info.Hints ?? 0);
            gameObject.SetActive(true);
        }

        public Tween Select()
        {
            return _selection.DOFade(1f, ViewValues.FAST_TIME_ANIMATION).OnStart(() => _selection.gameObject.SetActive(true));
        }

        public Tween Deselect()
        {
            return _selection.DOFade(0f, ViewValues.FAST_TIME_ANIMATION).OnComplete(() => _selection.gameObject.SetActive(false));
        }

        public Tween ActivateGlow()
        {
            return _glow.DOFade(1f, ViewValues.FAST_TIME_ANIMATION).OnStart(() => _glow.gameObject.SetActive(true));
        }

        public Tween DeactivateGlow()
        {
            return _glow.DOFade(0f, ViewValues.FAST_TIME_ANIMATION).OnComplete(() => _glow.gameObject.SetActive(false));
        }

        public void SetHealth(int amount) => _health.text = amount.ToString();

        public void SetSanity(int amount) => _sanity.text = amount.ToString();

        public void SetHints(int amount) => _hints.text = amount.ToString();

        public void ShowTurns(int amount) => _turnController.TurnOn(amount);

        private void SetPicture() => _picture.LoadCardSprite(Adventurer.AdventurerCard.Info.Code);

        /*******************************************************************/
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        async void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _activatorUIPresenter.HardDeactivate();
            await _swapAdventurerPresenter.Select(Adventurer);
            _activatorUIPresenter.HardActivate();
        }
    }
}
