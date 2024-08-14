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
        [Inject] private readonly TextsProvider _textsProvider;

        public bool CanBackToThisInteractable { get; private set; }
        public bool MustShowInCenter { get; private set; }
        public string Description { get; private set; }
        public BaseEffect EffectSelected { get; private set; }
        public BaseEffect MainButtonEffect { get; private set; }
        public BaseEffect UndoEffect { get; private set; }

        public bool IsUniqueEffect => _allCardEffects.Count() == 1;
        public bool IsUniqueCard => _allCardEffects.Select(effect => effect.CardOwner).UniqueOrDefault() != null;
        private bool NoEffect => (MainButtonEffect == null) && !_allCardEffects.Any();
        public bool IsMultiEffect => IsUniqueCard && !IsUniqueEffect;
        public Card UniqueCard => _allCardEffects.Select(effect => effect.CardOwner).Unique();
        private CardEffect UniqueCardEffect => _allCardEffects.Unique();
        public IEnumerable<CardEffect> AllEffects => _allCardEffects.ToList();
        public CardEffect GetUniqueEffect() => (MainButtonEffect == null && IsUniqueEffect) ? UniqueCardEffect : null;
        public BaseEffect GetUniqueMainButton() => MainButtonEffect != null && !_allCardEffects.Any() && MustShowInCenter ? MainButtonEffect : null;

        /*******************************************************************/
        public InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string localizableCode, params string[] localizableArgs)
        {
            CanBackToThisInteractable = canBackToThisInteractable;
            MustShowInCenter = mustShowInCenter;
            Description = _textsProvider.GetLocalizableText(localizableCode, localizableArgs);
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
            Investigator playedBy, string localizableCode, Card cardAffected = null, Stat resourceCost = null, params string[] localizableArgs)
        {
            string description = _textsProvider.GetLocalizableText(localizableCode, localizableArgs);
            CardEffect effect = new(card, activateTurnCost, logic, playActionType, playedBy, description, cardAffected, resourceCost: resourceCost);
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

        public BaseEffect CreateMainButton(Func<Task> logic, string localizableCode, params string[] localizableArgs)
        {
            string description = _textsProvider.GetLocalizableText(localizableCode, localizableArgs);
            BaseEffect effect = new(new Stat(0, false), logic, PlayActionType.None, null, description: description);
            MainButtonEffect = effect;
            return effect;
        }

        public void CreateContinueMainButton()
        {
            string description = _textsProvider.GetLocalizableText("MainButton_Continue");
            MainButtonEffect = new BaseEffect(new Stat(0, false), Continue, PlayActionType.None, null, description: description);
            static async Task Continue() => await Task.CompletedTask;
        }

        public void RemoveEffect(CardEffect effect) => _allCardEffects.Remove(effect);

        public void RemoveEffects(IEnumerable<CardEffect> effects) => effects.ForEach(effect => _allCardEffects.Remove(effect));

        private void SetUndoButton()
        {
            string description = _textsProvider.GetLocalizableText("MainButton_Back");
            UndoEffect = _gameActionsProvider.CanUndo() ? new BaseEffect(new Stat(0, false), UndoLogic, PlayActionType.None, null, description: description) : null;
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
