using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class BasicShowSelectorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _buttonPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private SelectorBlockController _selectorBlockController;
        [SerializeField, Required, SceneObjectsOnly] private TextMeshProUGUI _title;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TokensPileComponent _tokensPileComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        private List<CardView> _cardViews;
        public bool IsShowing => _selectorBlockController.IsActivated;
        public List<CardView> CardViewsOrdered(List<CardView> originalCards) =>
            originalCards.OrderBy(cardView => cardView.Card.CurrentZone.Cards.Count())
          .ThenBy(cardView => cardView.DeckPosition).ToList();

        /*******************************************************************/
        public async Task ShowCards(List<CardView> cardViews, string title)
        {
            _cardViews = cardViews;
            await ShowUp(title);
        }

        public async Task ShowDown(Tween restoreCards, bool withActivation)
        {
            if (!IsShowing) return;
            await _ioActivatorComponent.DeactivateCardSensors();
            Sequence returnSequence = DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_tokensPileComponent.RestorePosition())
                .Join(_selectorBlockController.DeactivateSelector())
                .Join(restoreCards);

            await returnSequence.AsyncWaitForCompletion();
            DeactiveTitle();
            if (withActivation) _ioActivatorComponent.ActivateCardSensors();
        }

        private async Task ShowUp(string title)
        {
            ActivateTitle(title);
            await _ioActivatorComponent.DeactivateCardSensors();
            Sequence showCenterSequence = DOTween.Sequence()
               .Append(_mainButtonComponent.MoveToShowSelector(_buttonPosition))
               .Join(_tokensPileComponent.MoveToShowSelector(_buttonPosition))
               .Join(_selectorBlockController.ActivateSelector());
            _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone, Ease.InSine)));
            await showCenterSequence.AsyncWaitForCompletion();
            _ioActivatorComponent.ActivateCardSensors();
        }

        private void ActivateTitle(string title)
        {
            _title.text = title;
            _title.gameObject.SetActive(true);
        }

        private void DeactiveTitle()
        {
            _title.gameObject.SetActive(false);
            _title.text = string.Empty;
        }
    }
}
