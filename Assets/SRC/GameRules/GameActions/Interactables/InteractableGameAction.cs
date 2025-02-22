using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        private readonly List<CardEffect> _allCardEffects = new();
        [Inject] private readonly IPresenterInteractable _interactablePresenter;

        public bool CanBackToThisInteractable { get; private set; }
        public bool MustShowInCenter { get; private set; }
        public Localization InteractableTitle { get; private set; }
        public BaseEffect EffectSelected { get; private set; }
        public BaseEffect MainButtonEffect { get; private set; }
        public BaseEffect UndoEffect { get; private set; }

        public bool IsUniqueEffect => AllPlayableEffects.Count() == 1;
        public bool IsUniqueCard => AllPlayableEffects.Select(effect => effect.CardOwner).UniqueOrDefault() != null;
        private bool NoEffect => (MainButtonEffect == null) && !AllPlayableEffects.Any();
        public bool IsMultiEffect => IsUniqueCard && !IsUniqueEffect;
        public Card UniqueCard => AllPlayableEffects.Select(effect => effect.CardOwner).Unique();
        private CardEffect UniqueCardEffect => AllPlayableEffects.Unique();
        public IEnumerable<CardEffect> AllPlayableEffects => _allCardEffects.FindAll(effect => effect.CanBePlayed);
        public CardEffect GetUniqueEffect() => (MainButtonEffect == null && IsUniqueEffect) ? UniqueCardEffect : null;
        public BaseEffect GetUniqueMainButton() => MainButtonEffect != null && !AllPlayableEffects.Any() && MustShowInCenter ? MainButtonEffect : null;

        /*******************************************************************/
        public InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization)
        {
            CanBackToThisInteractable = canBackToThisInteractable;
            MustShowInCenter = mustShowInCenter;
            InteractableTitle = localization;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            SetUndoButton();
            if (NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? GetUniqueMainButton() ?? await _interactablePresenter.SelectWith(this);
            await _gameActionsProvider.Create<PayRequerimentsEffectGameAction>().SetWith(EffectSelected).Execute();
            await _gameActionsProvider.Create<PlayEffectGameAction>().SetWith(EffectSelected).Execute();
        }

        /*******************************************************************/
        public IEnumerable<CardEffect> GetEffectForThisCard(Card cardAffected) => AllPlayableEffects.Where(effect => effect.CardOwner == cardAffected);

        public CardEffect CreateCardEffect(Card card, Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType,
            Investigator playedBy, Localization localization, Card cardAffected = null, Stat resourceCost = null)
        {
            CardEffect effect = new(card, activateTurnCost, logic, playActionType, playedBy, localization, cardAffected, resourceCost: resourceCost);
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

        public BaseEffect CreateMainButton(Func<Task> logic, Localization localization)
        {
            BaseEffect effect = new(new Stat(0, false), logic, PlayActionType.None, null, localization);
            MainButtonEffect = effect;
            return effect;
        }

        public void CreateContinueMainButton()
        {
            MainButtonEffect = new BaseEffect(new Stat(0, false), Continue, PlayActionType.None, null, new Localization("MainButton_Continue"));
            static async Task Continue() => await Task.CompletedTask;
        }

        public void RemoveEffect(CardEffect effect) => _allCardEffects.Remove(effect);

        public void RemoveEffects(IEnumerable<CardEffect> effects) => effects.ForEach(effect => _allCardEffects.Remove(effect));

        private void SetUndoButton()
        {
            UndoEffect = _gameActionsProvider.CanUndo() ? new BaseEffect(new Stat(0, false), UndoLogic, PlayActionType.None, null, new Localization("MainButton_Back")) : null;
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
