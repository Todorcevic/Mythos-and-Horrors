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
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public bool CanBackToThisInteractable { get; }
        public bool MustShowInCenter { get; }
        public string Description { get; }

        public Effect EffectSelected { get; private set; }
        public Effect MainButtonEffect { get; private set; }
        public Effect UndoEffect { get; private set; }
        private Effect UniqueEffect => _allEffects.Single();
        public bool IsUniqueEffect => _allEffects.Count() == 1;
        public bool IsUniqueCard => _allEffects.All(effect => effect.CardAffected == _allEffects.First().CardAffected);
        public Card UniqueCard => _allEffects.Select(effect => effect.CardAffected).Unique();
        private bool NoEffect => IsManadatary && _allEffects.Count() == 0;
        public bool IsManadatary => MainButtonEffect == null && UndoEffect == null;

        /*******************************************************************/
        public InteractableGameAction(bool canBackToThisInteractable, bool mustShowInCenter, string description)
        {
            CanBackToThisInteractable = canBackToThisInteractable;
            MustShowInCenter = mustShowInCenter;
            Description = description;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            SetUndoButton();
            if (NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            await _gameActionsProvider.Create(new PlayEffectGameAction(EffectSelected));
        }

        public Effect GetUniqueEffect()
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

        public Effect CreateUndoButton()
        {
            Effect effect = new();
            UndoEffect = effect;
            return effect;
        }

        private void SetUndoButton()
        {
            if (!_gameActionsProvider.CanUndo()) UndoEffect = null;
        }

        public void RemoveEffect(Effect effect) => _allEffects.Remove(effect);
    }
}
