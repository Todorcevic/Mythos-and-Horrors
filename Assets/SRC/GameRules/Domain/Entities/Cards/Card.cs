using ModestTree;
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
        private readonly List<Stat> _stats = new();
        private readonly List<Activation> _activations = new();
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
        public IEnumerable<Activation> AllActivations => _activations.ToList();
        public IEnumerable<Buff> AffectedByThisBuffs => _buffsProvider.GetBuffsAffectToThisCard(this);
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Count() > 0;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IEnumerable<Effect> PlayableEffects => _gameActionsProvider.CurrentInteractable?.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorOwnerWithThisZone(CurrentZone) ??
            _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public bool IsInPlay => ZoneType.PlayZone.HasFlag(CurrentZone.ZoneType);
        public bool IsActivable => _activations.Count > 0;
        public bool CanDiscard => !Tags.Contains(Tag.Flaw);
        public bool IsVictory => Info.Victory != null;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            OwnZone = _zonesProvider.Create(ZoneType.Own);
            FaceDown = new State(false);
            Exausted = new State(false);
            Blancked = new State(false, BlankState);
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
                interactableGameAction.Create().SetCard(this).SetInvestigator(Owner).SetLogic(FullLogic);
                await _gameActionsProvider.Create(interactableGameAction);

                /*******************************************************************/
                async Task Continue() => await Task.CompletedTask;
                async Task FullLogic() => await logic.Invoke(gameAction);
            }
        }

        /*******************************************************************/
        protected Activation CreateActivation(Stat activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition, bool isBase = false)
        {
            Activation newActivation = new(activateTurnsCost, logic, condition, isBase);
            _activations.Add(newActivation);
            return newActivation;
        }

        protected Stat CreateStat(int value, bool canBeNegative = false)
        {
            Stat newSAtat = new(value, canBeNegative);
            _stats.Add(newSAtat);
            return newSAtat;
        }

        public bool HasThisStat(Stat stat) => _stats.Contains(stat);

        /*******************************************************************/
        private List<Activation> _activationsBlanked = new();
        private List<Buff> _buffsCreatedBlanked = new();

        protected virtual void BlankState(bool isActive)
        {
            if (isActive)
            {
                _activationsBlanked = _activations.FindAll(activation => !activation.IsBase).ToList();
                _activations.Clear();
                _specificReactions.ForEach(reaction => reaction.Disable());
                _buffsCreatedBlanked = _buffsProvider.GetBuffsForThisCardMaster(this).ToList();
                _buffsProvider.Remove(_buffsCreatedBlanked);
            }
            else
            {
                _activations.AddRange(_activationsBlanked);
                _activationsBlanked.Clear();
                _specificReactions.ForEach(reaction => reaction.Enable());
                _buffsProvider.Add(_buffsCreatedBlanked);
                _buffsCreatedBlanked.Clear();
            }
        }
    }
}
