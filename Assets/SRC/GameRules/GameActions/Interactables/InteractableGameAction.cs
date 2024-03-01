using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        private Effect EffectSelected { get; set; }
        public bool IsManadatary { get; }

        /*******************************************************************/
        public InteractableGameAction(bool isMandatary)
        {
            IsManadatary = isMandatary;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CheckBuffs();
            if (_effectProvider.NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            ClearEffectsInAllCards();
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
            if (_effectProvider.IsUniqueEffect) return _effectProvider.UniqueEffect;
            return null;
        }

        private void ClearEffectsInAllCards() => _effectProvider.ClearAllEffects();
    }
}
