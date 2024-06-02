using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction, IInitializable
    {
        private readonly List<Effect> _allCardEffects = new();
        [Inject] private readonly IInteractablePresenter _interactablePresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public bool CanBackToThisInteractable { get; protected set; }
        public bool MustShowInCenter { get; protected set; }
        public virtual string Description { get; protected set; }

        public Investigator ActiveInvestigator { get; }
        public Effect EffectSelected { get; private set; }
        public Effect MainButtonEffect { get; private set; }
        public Effect UndoEffect { get; private set; }
        private Effect UniqueEffect => _allCardEffects.Unique();
        public bool IsUniqueEffect => _allCardEffects.Count() == 1;
        public bool IsUniqueCard => _allCardEffects.Select(effect => effect.Card).UniqueOrDefault() != null;
        public Card UniqueCard => _allCardEffects.Select(effect => effect.Card).Unique();
        private bool NoEffect => MainButtonEffect == null && _allCardEffects.Count() == 0;
        public bool IsManadatary => MainButtonEffect == null;
        public bool IsMultiEffect => IsUniqueCard && !IsUniqueEffect;
        public IEnumerable<Effect> AllEffects => _allCardEffects.ToList();


        /*******************************************************************/
        public InteractableGameAction(bool canBackToThisInteractable, bool mustShowInCenter, string description, Investigator activeInvestigator = null)
        {
            CanBackToThisInteractable = canBackToThisInteractable;
            MustShowInCenter = mustShowInCenter;
            Description = description;
            ActiveInvestigator = activeInvestigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            SetUndoButton();
            if (NoEffect) return;
            EffectSelected = GetUniqueEffect() ?? await _interactablePresenter.SelectWith(this);
            await _gameActionsProvider.Create(new PlayEffectGameAction(EffectSelected));
        }

        //public Effect AutoPlay()
        //{
        //    if (MainButtonEffect != null) return null;
        //    if (!IsUniqueEffect) return null;
        //    return UniqueEffect;
        //}

        public Effect GetUniqueEffect()
        {
            if (!IsManadatary) return null;
            if (!IsUniqueEffect) return null;
            return UniqueEffect;
        }

        /*******************************************************************/
        public IEnumerable<Effect> GetEffectForThisCard(Card cardAffected) => _allCardEffects.FindAll(effect => effect.Card == cardAffected);

        public Effect Create()
        {
            Effect effect = new();
            _allCardEffects.Add(effect);
            return effect;
        }

        public Effect CreateMainButton()
        {
            Effect effect = new();
            MainButtonEffect = effect;
            return effect;
        }

        public void CreateCancelMainButton()
        {
            MainButtonEffect = new Effect().SetLogic(UndoLogic).SetDescription("Cancel");
        }

        private void SetUndoButton()
        {
            UndoEffect = _gameActionsProvider.CanUndo() ? new Effect().SetLogic(UndoLogic).SetDescription("Back") : null;
        }

        async Task UndoLogic()
        {
            InteractableGameAction lastInteractable = await _gameActionsProvider.UndoLastInteractable();
            if (lastInteractable.GetType() != typeof(InteractableGameAction)) lastInteractable.ClearEffects();
            await _gameActionsProvider.Create(lastInteractable);
        }

        public void RemoveEffect(Effect effect) => _allCardEffects.Remove(effect);

        public void ClearEffects() => _allCardEffects.Clear();

        public virtual void ExecuteSpecificInitialization() { }
    }
}
