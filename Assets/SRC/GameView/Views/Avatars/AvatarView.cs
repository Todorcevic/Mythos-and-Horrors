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
        [Inject] private readonly SwapInvestigatorPresenter _swapInvestigatorPresenter;
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;

        public bool IsVoid => Investigator == null;
        public Investigator Investigator { get; private set; }

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

        private void SetPicture() => _picture.LoadAvatarSprite(Investigator.InvestigatorCard.Info.Code);

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
            _activatorUIPresenter.Deactivate();
            await _swapInvestigatorPresenter.Select(Investigator);
            _activatorUIPresenter.Activate();
        }
    }
}
