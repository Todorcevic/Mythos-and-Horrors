using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        private readonly List<CardEffect> _allCardEffects = new();
        [Inject] private readonly IInteractablePresenter _interactablePresenter;

        public bool CanBackToThisInteractable { get; private set; }
        public bool MustShowInCenter { get; private set; }
        public string Code { get; private set; }

        public BaseEffect EffectSelected { get; private set; }
        public BaseEffect MainButtonEffect { get; private set; }
        public BaseEffect UndoEffect { get; private set; }
        private CardEffect UniqueEffect => _allCardEffects.Unique();
        public bool IsUniqueEffect => _allCardEffects.Count() == 1;
        public bool IsUniqueCard => _allCardEffects.Select(effect => effect.CardOwner).UniqueOrDefault() != null;
        public Card UniqueCard => _allCardEffects.Select(effect => effect.CardOwner).Unique();
        private bool NoEffect => (MainButtonEffect == null) && !_allCardEffects.Any();
        private bool JustMainButton => MainButtonEffect != null && !_allCardEffects.Any() && MustShowInCenter;
        public bool IsMandatary => MainButtonEffect == null;
        public bool IsMultiEffect => IsUniqueCard && !IsUniqueEffect;
        public IEnumerable<CardEffect> AllEffects => _allCardEffects.ToList();
        public CardEffect GetUniqueEffect() => (IsMandatary && IsUniqueEffect) ? UniqueEffect : null;
        public BaseEffect GetUniqueMainButton() => JustMainButton ? MainButtonEffect : null;

        /*******************************************************************/
        public InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string code)
        {
            CanBackToThisInteractable = canBackToThisInteractable;
            MustShowInCenter = mustShowInCenter;
            Code = code;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            SetUndoButton();
            if (NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? GetUniqueMainButton() ?? await _interactablePresenter.SelectWith(this);
            await _gameActionsProvider.Create<PlayEffectGameAction>().SetWith(EffectSelected).Execute();
        }

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

        public void CreateContinueMainButton()
        {
            MainButtonEffect = new BaseEffect(new Stat(0, false), Continue, PlayActionType.None, null, description: "Continue");
            static async Task Continue() => await Task.CompletedTask;
        }

        public void RemoveEffect(CardEffect effect) => _allCardEffects.Remove(effect);

        public void RemoveEffects(IEnumerable<CardEffect> effects) => effects.ForEach(effect => _allCardEffects.Remove(effect));

        private void SetUndoButton()
        {
            UndoEffect = _gameActionsProvider.CanUndo() ? new BaseEffect(new Stat(0, false), UndoLogic, PlayActionType.None, null, description: "Back") : null;
            if (MainButtonEffect == null) MainButtonEffect = UndoEffect;

            /*******************************************************************/
            async Task UndoLogic()
            {
                InteractableGameAction lastInteractable = await _gameActionsProvider.UndoLastInteractable();
                await lastInteractable.Execute();
            }
        }
    }
}
