using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        [Inject] private readonly INewPresenter<InteractableGameAction> _returnCardPresenter;
        [Inject] private readonly ViewLayersProvider _viewLayersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public List<Card> ActivableCards => _cardsProvider.GetPlayableCards();
        public bool IsManadatary { get; }
        public Effect EffectSelected { get; private set; }
        public bool NothingIsSelected => EffectSelected == null;
        public IEnumerable<Effect> AllEffects => ActivableCards.SelectMany(card => card.PlayableEffects);
        public bool IsUniqueEffect => AllEffects.Count() == 1;
        public bool NoEffect => AllEffects.Count() == 0;

        /*******************************************************************/
        public InteractableGameAction(bool isMandatary)
        {
            IsManadatary = isMandatary;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CheckBuffs();
            if (NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? await _viewLayersProvider.StartSelectionWith(this);
            ClearEffectsInAllCards();
            if (NothingIsSelected) return;
            await _gameActionFactory.Create(new PlayEffectGameAction(EffectSelected));
            await _returnCardPresenter.PlayAnimationWith(this);
        }

        private void CheckBuffs()
        {
            _reactionablesProvider.CheckActivationBuffs();
            _reactionablesProvider.CheckDeactivationBuffs();
        }

        private Effect GetUniqueEffect()
        {
            if (!IsManadatary) return null;
            if (IsUniqueEffect) return AllEffects.First();
            return null;
        }

        private void ClearEffectsInAllCards() => _cardsProvider.AllCards.ForEach(card => card.ClearEffects());
    }
}
