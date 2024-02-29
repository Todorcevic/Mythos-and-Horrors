using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        private List<IEffectable> ActivableCards => _cardsProvider.GetPlayableCards();
        private IEnumerable<Effect> AllEffects => ActivableCards.SelectMany(effectable => effectable.PlayableEffects);
        private bool IsUniqueEffect => AllEffects.Count() == 1;
        private bool NoEffect => AllEffects.Count() == 0;
        private Effect EffectSelected { get; set; }

        public bool IsManadatary { get; }
        public bool NothingIsSelected => EffectSelected == null;


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
            EffectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            ClearEffectsInAllCards();
            if (NothingIsSelected) return;
            await _gameActionFactory.Create(new PlayEffectGameAction(EffectSelected));
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

        private void ClearEffectsInAllCards() => _effectProvider.ClearAllEffects();
    }
}
