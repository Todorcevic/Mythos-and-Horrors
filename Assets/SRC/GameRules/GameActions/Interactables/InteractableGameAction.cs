using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction, IInitializable
    {
        private readonly List<CardEffect> _allCardEffects = new();
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public bool CanBackToThisInteractable { get; protected set; }
        public bool MustShowInCenter { get; protected set; }
        public virtual string Description { get; protected set; }

        public BaseEffect EffectSelected { get; private set; }
        public BaseEffect MainButtonEffect { get; private set; }
        public BaseEffect UndoEffect { get; private set; }
        private CardEffect UniqueEffect => _allCardEffects.Unique();
        public bool IsUniqueEffect => _allCardEffects.Count() == 1;
        public bool IsUniqueCard => _allCardEffects.Select(effect => effect.CardOwner).UniqueOrDefault() != null;
        public Card UniqueCard => _allCardEffects.Select(effect => effect.CardOwner).Unique();
        private bool NoEffect => MainButtonEffect == null && !_allCardEffects.Any();
        private bool JustMainButton => MainButtonEffect != null && !_allCardEffects.Any() && MustShowInCenter;
        public bool IsManadatary => MainButtonEffect == null;
        public bool IsMultiEffect => IsUniqueCard && !IsUniqueEffect;
        public IEnumerable<CardEffect> AllEffects => _allCardEffects.ToList();


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
            EffectSelected = GetUniqueEffect() ?? GetUniqueMainButton() ?? await _interactablePresenter.SelectWith(this);
            await _gameActionsProvider.Create(new PlayEffectGameAction(EffectSelected));
        }

        public CardEffect GetUniqueEffect() => (IsManadatary && IsUniqueEffect) ? UniqueEffect : null;
        public BaseEffect GetUniqueMainButton() => JustMainButton ? MainButtonEffect : null;

        /*******************************************************************/
        public IEnumerable<CardEffect> GetEffectForThisCard(Card cardAffected) => _allCardEffects.FindAll(effect => effect.CardOwner == cardAffected);

        public CardEffect CreateEffect(Card card, Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType,
            Investigator playedBy, Card cardAffected = null, Stat resourceCost = null)
        {
            CardEffect effect = new(card, activateTurnCost, logic, playActionType, playedBy, cardAffected, resourceCost: resourceCost);
            if (CanBeAdded(effect)) _allCardEffects.Add(effect);
            return effect;

            /*******************************************************************/
            bool CanBeAdded(CardEffect effect)
            {
                if (this is not IPersonalInteractable personalInteractable) return true;
                if (personalInteractable.ActiveInvestigator.Isolated.IsActive && effect.Investigator != personalInteractable.ActiveInvestigator) return false;
                return true;
            }
        }

        public BaseEffect CreateMainButton(Func<Task> logic, string description)
        {
            BaseEffect effect = new(new Stat(0, false), logic, PlayActionType.None, null, description: description);
            MainButtonEffect = effect;
            return effect;
        }

        public void CreateCancelMainButton()
        {
            MainButtonEffect = new BaseEffect(new Stat(0, false), UndoLogic, PlayActionType.None, null, description: "Cancel");
        }

        public void CreateContinueMainButton()
        {
            MainButtonEffect = new BaseEffect(new Stat(0, false), Continue, PlayActionType.None, null, description: "Continue");
            static async Task Continue() => await Task.CompletedTask;
        }

        private void SetUndoButton()
        {
            UndoEffect = _gameActionsProvider.CanUndo() ? new BaseEffect(new Stat(0, false), UndoLogic, PlayActionType.None, null, description: "Back") : null;
        }

        async Task UndoLogic()
        {
            InteractableGameAction lastInteractable = await _gameActionsProvider.UndoLastInteractable();
            if (lastInteractable.GetType() != typeof(InteractableGameAction)) lastInteractable.ClearEffects();
            await _gameActionsProvider.Create(lastInteractable);
        }

        public void RemoveEffect(CardEffect effect) => _allCardEffects.Remove(effect);

        public void RemoveEffects(IEnumerable<CardEffect> effects) => effects.ForEach(effect => _allCardEffects.Remove(effect));

        public void ClearEffects() => _allCardEffects.Clear();

        public virtual void ExecuteSpecificInitialization() { }
    }
}
