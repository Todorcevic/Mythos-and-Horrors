using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        private readonly List<Effect> _allEffects = new();
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public Effect EffectSelected { get; private set; }
        public Effect MainButtonEffect { get; private set; }
        public Effect UndoEffect { get; private set; }
        private Effect UniqueEffect => _allEffects.Single();
        private bool IsUniqueEffect => _allEffects.Count() == 1;
        private bool NoEffect => _allEffects.Count() == 0;
        public bool IsManadatary => MainButtonEffect == null;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            await _gameActionsProvider.Create(new PlayEffectGameAction(EffectSelected));
        }

        private Effect GetUniqueEffect()
        {
            if (!IsManadatary) return null;
            if (IsUniqueEffect) return UniqueEffect;
            return null;
        }
        /*******************************************************************/
        public IEnumerable<Effect> GetEffectForThisCard(Card cardAffected) => _allEffects.FindAll(effect => effect.CardAffected == cardAffected);

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

        public void WithUndoButton()
        {
            UndoEffect = new();
            UndoEffect.SetLogic(RealUndoEffect);

            async Task RealUndoEffect()
            {
                InteractableGameAction lastPlayInvestigator = await _gameActionsProvider.UndoLastInteractable();
                _gameActionsProvider.GetLastActive<PlayInvestigatorGameAction>()?.Stop();
                await _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorProvider.ActiveInvestigator));
            }
        }

        public void RemoveEffect(Effect effect) => _allEffects.Remove(effect);
    }
}
