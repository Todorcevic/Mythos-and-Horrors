using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        private bool _mustBeStopped;
        private readonly List<Effect> _allEffects = new();
        private Effect _effectSelected;
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Effect MainButtonEffect { get; private set; }
        private Effect UniqueEffect => _allEffects.Single();
        private bool IsUniqueEffect => _allEffects.Count() == 1;
        private bool NoEffect => _allEffects.Count() == 0;
        public bool IsManadatary => MainButtonEffect == null;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (NoEffect) return;
            _effectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            if (_mustBeStopped)
            {
                return;
            }
            await _gameActionsProvider.Create(new PlayEffectGameAction(_effectSelected));
        }

        private Effect GetUniqueEffect()
        {
            if (!IsManadatary) return null;
            if (IsUniqueEffect) return UniqueEffect;
            return null;
        }
        /*******************************************************************/

        public IEnumerable<Effect> GetEffectForThisCard(Card cardAffected) =>
           _allEffects.FindAll(effect => effect.CardAffected == cardAffected);

        public Effect Create()
        {
            Effect effect = new();
            _allEffects.Add(effect);
            return effect;
        }

        public Effect CreateMainButton()
        {
            Effect effect = new();
            MainButtonEffect = effect;
            return effect;
        }

        public void RemoveEffect(Effect effect) => _allEffects.Remove(effect);

        public void Stop() => _mustBeStopped = true;
    }
}
