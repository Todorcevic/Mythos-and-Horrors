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
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        private readonly List<Stat> _stats = new();
        private readonly List<State> _states = new();
        private readonly List<Activation> _baseActivations = new();
        private readonly List<Activation> _specificActivations = new();
        private readonly List<IReaction> _baseReactions = new();
        private readonly List<IReaction> _specificReactions = new();
        private readonly List<Buff> _baseBuffs = new();
        private readonly List<Buff> _specificBuffss = new();

        public Stat ExtraStat { get; protected set; }
        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public State Blancked { get; private set; }
        public Zone OwnZone { get; private set; }

        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public virtual IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();
        public IEnumerable<Activation> AllActivations => _baseActivations.Concat(_specificActivations);
        public IEnumerable<Buff> AllBuffs => _baseBuffs.Concat(_specificBuffss);
        public IEnumerable<Buff> AffectedByThisBuffs => _buffsProvider.GetBuffsAffectToThisCard(this);
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Any();
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IEnumerable<CardEffect> PlayableEffects => _gameActionsProvider.CurrentInteractable?.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorOnlyZonesOwnerWithThisZone(CurrentZone) ??
            _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public Investigator ControlOwner => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone);
        public bool IsInPlay => ZoneType.PlayZone.HasFlag(CurrentZone.ZoneType);
        public bool IsActivable => AllActivations.Any();
        public bool CanBeDiscarded => !HasThisTag(Tag.Weakness);
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
        }

        /************************** REACTIONS ******************************/
        protected Reaction<T> CreateOptativeReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart, Stat resourceCost = null,
            PlayActionType playActionType = PlayActionType.Choose) where T : GameAction
        {
            Reaction<T> newReaction = _reactionablesProvider.CreateReaction(condition, OptativeLogic, isAtStart);
            _specificReactions.Add(newReaction);
            return newReaction;

            async Task OptativeLogic(T gameAction)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true,
                    mustShowInCenter: true,
                    "Optative Reaction",
                    ControlOwner ?? _investigatorsProvider.Leader);
                interactableGameAction.CreateContinueMainButton();
                interactableGameAction.CreateEffect(this,
                    new Stat(0, false),
                    FullLogic,
                    playActionType,
                    playedBy: ControlOwner ?? _investigatorsProvider.Leader,
                    resourceCost: resourceCost);
                await _gameActionsProvider.Create(interactableGameAction);

                /*******************************************************************/
                async Task FullLogic() => await logic.Invoke(gameAction);
            }
        }

        protected Reaction<T> CreateOneTimeReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart, bool isBase = false) where T : GameAction
        {
            Reaction<T> newReaction = null;
            newReaction = _reactionablesProvider.CreateReaction(condition, OneTimeLogic, isAtStart);
            _specificReactions.Add(newReaction);
            return newReaction;

            async Task OneTimeLogic(T gameAction)
            {
                _specificReactions.Remove(newReaction);
                await logic.Invoke(gameAction);
            }
        }

        protected Reaction<T> CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart, bool isBase = false) where T : GameAction
        {
            Reaction<T> newReaction = _reactionablesProvider.CreateReaction(condition, logic, isAtStart);
            if (isBase) _baseReactions.Add(newReaction);
            else _specificReactions.Add(newReaction);
            return newReaction;
        }

        /***************************** ACTIVATIONS *****************************/
        protected Activation CreateActivation(Stat activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition, PlayActionType playActionType,
            bool isBase = false)
        {
            Activation newActivation = new(activateTurnsCost, new GameCommand<Investigator>(logic), new GameCondition<Investigator>(condition), playActionType);
            if (isBase) _baseActivations.Add(newActivation);
            else _specificActivations.Add(newActivation);
            return newActivation;
        }

        /***************************** BUFFS *****************************/
        protected Buff CreateBuff(Func<IEnumerable<Card>> cardsToBuff, Func<IEnumerable<Card>, Task> activationLogic,
            Func<IEnumerable<Card>, Task> deactivationLogic, bool isBase = false)
        {
            Buff newBuff = new(this, cardsToBuff, new GameCommand<IEnumerable<Card>>(activationLogic), new GameCommand<IEnumerable<Card>>(deactivationLogic));
            if (isBase) _baseBuffs.Add(newBuff);
            else _specificBuffss.Add(newBuff);
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
            if (isActive)
            {
                _specificActivations.ForEach(activation => activation.Disable());
                _specificReactions.ForEach(reaction => reaction.Disable());
                _specificBuffss.ForEach(buff => buff.Disable());
            }
            else
            {
                _specificActivations.ForEach(activation => activation.Enable());
                _specificReactions.ForEach(reaction => reaction.Enable());
                _specificBuffss.ForEach(buff => buff.Enable());
            }
        }
    }
}
