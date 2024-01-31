using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowSelectorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SelectorBlockController _selectorBlockController;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _buttonPosition;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        private List<CardView> _cardViews = new();

        public bool IsShowing => _selectorBlockController.IsActivated;
        public bool IsMultiEffect => _cardViews.Count > 1 && _cardViews.All(cardView => cardView.Card == _cardViews[0].Card);

        /*******************************************************************/
        public Tween ShowPlayables()
        {
            _cardViews = _cardViewsManager.GetAllCanPlay();
            _cardViews.ForEach(cardView => cardView.ShowEffects());
            return Animation();
        }

        public async Task ReturnPlayables(CardView exceptThis = null)
        {
            if (!IsShowing) return; // Guard clause for InteractablePresenter when the player click a card
            Sequence returnSequence = DOTween.Sequence().Append(Shutdown());
            _cardViews.Except(new CardView[] { exceptThis })
                .OrderBy(cardView => cardView.DeckPosition).ToList()
                .ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone), Ease.Linear)));
            await returnSequence.AsyncWaitForCompletion();
            _cardViews.Clear();
        }

        /*******************************************************************/
        public Tween ShowMultiEffects(Dictionary<CardView, Effect> cardViews)
        {
            _cardViews = cardViews.Keys.ToList();
            _cardViews.ForEach(cardView => cardView.ShowEffect(cardViews[cardView]));
            _mainButtonComponent.Activate(ViewText.BUTTON_BACK);
            return Animation();
        }

        public async Task ReturnClones()
        {
            CardView originalCardView = _cardViews[0];
            List<CardView> clones = _cardViews.Except(new[] { originalCardView }).OrderBy(cardView => cardView.DeckPosition).ToList();
            Sequence returnClonesSequence = DOTween.Sequence().Append(Shutdown());
            clones.ForEach(clone => returnClonesSequence.Join(clone.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.OutSine)
                 .OnComplete(() => Destroy(clone.gameObject))));
            await returnClonesSequence.AsyncWaitForCompletion()
                .Join(_moveCardHandler.MoveCardWithPreviewToZone(originalCardView, _zoneViewsManager.Get(originalCardView.Card.CurrentZone)));
            _cardViews.Clear();
        }

        public Tween DestroyClones(CardView cardViewSelected)
        {
            CardView originalCardView = _cardViews[0];
            List<CardView> clones = _cardViews.Except(new[] { originalCardView }).OrderBy(cardView => cardView.DeckPosition).ToList();
            (originalCardView.transform.position, cardViewSelected.transform.position) = (cardViewSelected.transform.position, originalCardView.transform.position);
            Sequence sequence = DOTween.Sequence().Append(Shutdown());
            clones.ForEach(clone => sequence.Join(clone.MoveToZone(_zoneViewsManager.OutZone))
                 .OnComplete(() => Destroy(clone.gameObject)));
            _cardViews.Clear();
            return sequence;
        }

        /*******************************************************************/
        private Tween Shutdown()
        {
            _cardViews.ForEach(cardView => cardView.HideEffects());
            return DOTween.Sequence()
                .Append(_mainButtonComponent.RestorePosition())
                .Join(_selectorBlockController.DeactivateSelector());
        }

        private Tween Animation()
        {
            Sequence showCenterSequence = DOTween.Sequence()
               .Append(_mainButtonComponent.MoveToThis(_buttonPosition).SetEase(Ease.Linear))
               .Join(_selectorBlockController.ActivateSelector());
            _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone, Ease.Linear)));
            return showCenterSequence;
        }
    }
}
