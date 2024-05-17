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
        [Inject] protected readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private readonly List<Stat> _stats = new();
        private readonly List<State> _states = new();
        private readonly List<Activation> _baseActivations = new();
        private readonly List<Activation> _specificActivations = new();
        private readonly List<IReaction> _baseReactions = new();
        private readonly List<IReaction> _specificReactions = new();

        public Stat ExtraStat { get; protected set; }
        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public State Blancked { get; private set; }
        public Zone OwnZone { get; private set; }

        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public virtual IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();
        public IEnumerable<Activation> AllActivations => _baseActivations.Concat(_specificActivations);
        public IEnumerable<Buff> AffectedByThisBuffs => _buffsProvider.GetBuffsAffectToThisCard(this);
        public IEnumerable<Buff> Buffs => _buffsProvider.GetBuffsForThisCardMaster(this);
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Count() > 0;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IEnumerable<Effect> PlayableEffects => _gameActionsProvider.CurrentInteractable?.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorOwnerWithThisZone(CurrentZone) ??
            _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public bool IsInPlay => ZoneType.PlayZone.HasFlag(CurrentZone.ZoneType);
        public bool IsActivable => AllActivations.Count() > 0;
        public bool CanBeDiscarded => !Tags.Contains(Tag.Weakness);
        public bool IsVictory => Info.Victory != null;
        public bool HasThisTag(Tag tag) => Tags.Contains(tag);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            OwnZone = _zonesProvider.Create(ZoneType.Own);
            _zonesProvider.OutZone.AddCard(this);
            FaceDown = CreateState(false);
            Exausted = CreateState(false);
            Blancked = CreateState(false, BlankState);
        }
        /*******************************************************************/
        protected void CreateReaction<T>(Func<T, bool> condition, Func<T, Task> logic, bool isAtStart,
            bool isBase = false, bool isOptative = false) where T : GameAction
        {
            IReaction newReaction = _reactionablesProvider.CreateReaction(condition, isOptative ? OptativeLogic : logic, isAtStart);
            if (isBase) _baseReactions.Add(newReaction);
            else _specificReactions.Add(newReaction);

            async Task OptativeLogic(T gameAction)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Optative Reaction");
                interactableGameAction.CreateMainButton().SetLogic(Continue);
                interactableGameAction.Create().SetCard(this).SetInvestigator(Owner).SetLogic(FullLogic).SetDescription(logic.Method.Name);
                await _gameActionsProvider.Create(interactableGameAction);

                /*******************************************************************/
                async Task Continue() => await Task.CompletedTask;
                async Task FullLogic() => await logic.Invoke(gameAction);
            }
        }

        protected Activation CreateActivation(Stat activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition,
            bool isBase = false, bool withOpportunityAttck = true)
        {
            Activation newActivation = new(activateTurnsCost, logic, condition, withOpportunityAttck);
            if (isBase) _baseActivations.Add(newActivation);
            else _specificActivations.Add(newActivation);
            return newActivation;
        }

        protected Activation CreateFreeActivation(Func<Investigator, Task> logic, Func<Investigator, bool> condition, bool isBase = false)
        {
            return CreateActivation(CreateStat(0), logic, condition, isBase: isBase, withOpportunityAttck: false);
        }

        protected Stat CreateStat(int value, bool canBeNegative = false)
        {
            Stat newStat = new(value, canBeNegative);
            _stats.Add(newStat);
            return newStat;
        }

        public bool HasThisStat(Stat stat) => _stats.Contains(stat);

        protected State CreateState(bool value, Action<bool> action = null)
        {
            State newState = new(value, action);
            _states.Add(newState);
            return newState;
        }

        public bool HasThisState(State state) => _states.Contains(state);

        /*******************************************************************/
        protected virtual void BlankState(bool isActive)
        {
            if (isActive)
            {
                _specificActivations.ForEach(activation => activation.Disable());
                _specificReactions.ForEach(reaction => reaction.Disable());
                _buffsProvider.GetBuffsForThisCardMaster(this).ForEach(buff => buff.Disable());
            }
            else
            {
                _specificActivations.ForEach(activation => activation.Enable());
                _specificReactions.ForEach(reaction => reaction.Enable());
                _buffsProvider.GetBuffsForThisCardMaster(this).ForEach(buff => buff.Enable());
            }
        }
    }
}
