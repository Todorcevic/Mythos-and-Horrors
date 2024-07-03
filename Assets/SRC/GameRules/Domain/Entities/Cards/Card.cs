using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card
    {
        [Inject] private readonly CardInfo _info;
        [InjectOptional] private readonly CardExtraInfo _extraInfo;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly OptativeReactionsProvider _realReactionsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly ActivationsProvider _activationsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        private readonly List<Stat> _stats = new();
        private readonly List<State> _states = new();
        protected readonly List<IAbility> _specificAbilities = new();

        public Stat ExtraStat { get; protected set; }
        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public State Blancked { get; private set; }
        public Conditional CanBeDiscarted { get; private set; }
        public Zone OwnZone { get; private set; }

        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public virtual IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();
        public IEnumerable<Activation<Investigator>> AllActivations => _activationsProvider.GetActivationsFor(this);
        public IEnumerable<Buff> AllBuffs => _buffsProvider.GetBuffsForThisCardMaster(this);
        public IEnumerable<Buff> AffectedByThisBuffs => _buffsProvider.GetBuffsAffectToThisCard(this);
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Any();
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IEnumerable<CardEffect> PlayableEffects => _gameActionsProvider.CurrentInteractable?.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorOnlyZonesOwnerWithThisZone(CurrentZone) ??
            _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public virtual Investigator ControlOwner => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone);
        public virtual bool IsInPlay => ZoneType.PlayZone.HasFlag(CurrentZone.ZoneType);
        public bool IsActivable => AllActivations.Any();
        private bool CanBeDiscarded
        {
            get
            {
                if (this is IPermanentable) return false;
                if (HasThisTag(Tag.Weakness)) return false;
                return true;
            }
        }

        public bool IsVictory => Info.Victory != null;
        public bool HasThisTag(Tag tag) => Tags.Contains(tag);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            OwnZone = new(ZoneType.Own);
            _zonesProvider.OutZone.AddCard(this);
            FaceDown = CreateState(false);
            Exausted = CreateState(false);
            Blancked = CreateState(false, BlankState);
            CanBeDiscarted = new(() => CanBeDiscarded);
        }

        /************************** REACTIONS ******************************/
        protected OptativeReaction<T> CreateOptativeReaction<T>(Func<T, bool> condition, Func<T, Task> logic, GameActionTime time,
            PlayActionType playActionType = PlayActionType.None) where T : GameAction
        {
            OptativeReaction<T> realReaction = _realReactionsProvider.CreateOptativeReaction(this, condition, logic, time, playActionType);
            _specificAbilities.Add(realReaction);
            return realReaction;
        }

        protected Reaction<T> CreateBaseReaction<T>(Func<T, bool> condition, Func<T, Task> logic, GameActionTime time) where T : GameAction
        {
            Reaction<T> newReaction = _reactionablesProvider.CreateReaction(condition, logic, time);
            return newReaction;
        }

        protected Reaction<T> CreateForceReaction<T>(Func<T, bool> condition, Func<T, Task> logic, GameActionTime time, bool isBase = false) where T : GameAction
        {
            Reaction<T> newReaction = _reactionablesProvider.CreateReaction(condition, logic, time);
            if (!isBase) _specificAbilities.Add(newReaction);
            return newReaction;
        }

        public Reaction<T> CreateOneTimeReaction<T>(Func<T, bool> condition, Func<T, Task> logic, GameActionTime time) where T : GameAction
        {
            Reaction<T> newReaction = null;
            newReaction = _reactionablesProvider.CreateReaction(condition, OneTimeLogic, time);
            return newReaction;

            async Task OneTimeLogic(T gameAction)
            {
                _reactionablesProvider.RemoveReaction(newReaction);
                await logic.Invoke(gameAction);
            }
        }

        /***************************** ACTIVATIONS *****************************/
        protected Activation<Investigator> CreateActivation(int activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition, PlayActionType playActionType, Func<Card> cardAffected = null)
        {
            Activation<Investigator> newActivation = _activationsProvider.CreateActivation(this, activateTurnsCost, logic, condition, playActionType, cardAffected);
            _specificAbilities.Add(newActivation);
            return newActivation;
        }

        protected Activation<Investigator> CreateFastActivation(Func<Investigator, Task> logic, Func<Investigator, bool> condition, PlayActionType playActionType, Func<Card> cardAffected = null)
        {
            return CreateActivation(0, logic, condition, playActionType, cardAffected);
        }

        /***************************** BUFFS *****************************/
        protected Buff CreateBuff(Func<IEnumerable<Card>> cardsToBuff, Func<IEnumerable<Card>, Task> activationLogic, Func<IEnumerable<Card>, Task> deactivationLogic, bool isBase = false)
        {
            Buff newBuff = _buffsProvider.CreateBuff(this, cardsToBuff, activationLogic, deactivationLogic);
            _specificAbilities.Add(newBuff);
            return newBuff;
        }

        /*************************** STAT *********************************/
        protected Stat CreateStat(int value, bool canBeNegative = false)
        {
            Stat newStat = new(value, canBeNegative);
            _stats.Add(newStat);
            return newStat;
        }

        protected void RemoveStat(Stat stat)
        {
            _stats.Remove(stat);
        }

        public bool HasThisStat(Stat stat) => _stats.Contains(stat);

        /************************** STATE ********************************/
        protected State CreateState(bool value, Action<bool> action = null, bool isReseteable = true)
        {
            State newState = new(value, action);
            _states.Add(newState);
            return newState;
        }

        public bool HasThisState(State state) => _states.Contains(state);

        /*************************** LOGIC *******************************/
        protected virtual void BlankState(bool isActive)
        {
            if (isActive) _specificAbilities.ForEach(ability => ability.Disable());
            else _specificAbilities.ForEach(ability => ability.Enable());
        }
    }
}
