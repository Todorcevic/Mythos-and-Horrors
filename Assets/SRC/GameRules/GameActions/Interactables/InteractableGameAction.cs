using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        private Effect _effectSelected;
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly EffectsProvider _effectProvider;    

        public bool IsManadatary => _effectProvider.MainButtonEffect == null;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CheckBuffs();
            if (_effectProvider.NoEffect) return;
            _effectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            ClearEffectsInAllCards();
            await _gameActionFactory.Create(new PlayEffectGameAction(_effectSelected));
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
