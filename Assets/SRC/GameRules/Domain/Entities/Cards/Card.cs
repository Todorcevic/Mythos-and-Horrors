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
        private readonly List<Activation> _activations = new();
        private readonly List<Stat> _stats = new();
        private readonly List<IReaction> _beginReactions = new();
        private readonly List<IReaction> _finishReactions = new();

        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public Zone OwnZone { get; private set; }


        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public virtual IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();
        public IEnumerable<Activation> AllActivations => _activations.ToList();
        public IEnumerable<Buff> Buffs => _buffsProvider.GetBuffsForThisCard(this);
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Count() > 0;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IEnumerable<Effect> PlayableEffects => _gameActionsProvider.CurrentInteractable?.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorOwnerWithThisZone(CurrentZone) ??
            _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public bool IsInPlay => ZoneType.PlayZone.HasFlag(CurrentZone.ZoneType);
        public bool IsActivable => _activations.Count > 0;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            OwnZone = _zonesProvider.Create(ZoneType.Own);
            FaceDown = new State(false);
            Exausted = new State(false);

            _reactionablesProvider.SubscribeAtStart(WhenBegin);
            _reactionablesProvider.SubscribeAtEnd(WhenFinish);
        }

        private async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in _beginReactions)
                await reaction.React(gameAction);
        }

        private async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in _finishReactions)
                await reaction.React(gameAction);
        }

        /*******************************************************************/
        protected Reaction<T> CreateBeginReaction<T>(Func<T, bool> condition, Func<T, Task> logic) where T : GameAction
        {
            Reaction<T> newReaction = new(condition, logic);
            _beginReactions.Add(newReaction);
            return newReaction;
        }

        protected Reaction<T> CreateFinishReaction<T>(Func<T, bool> condition, Func<T, Task> logic) where T : GameAction
        {
            Reaction<T> newReaction = new(condition, logic);
            _finishReactions.Add(newReaction);
            return newReaction;
        }

        protected Reaction<T> CreateBeginOptativeReaction<T>(Func<T, bool> condition, Func<T, Task> logic) where T : GameAction
        {
            Reaction<T> newReaction = new(condition, RealLogic);
            _beginReactions.Add(newReaction);
            return newReaction;

            async Task RealLogic(GameAction gameAction)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Optative Reaction");
                interactableGameAction.CreateMainButton().SetLogic(Continue);
                interactableGameAction.Create().SetCard(this).SetInvestigator(Owner).SetLogic(() => FullLogic(gameAction));
                await _gameActionsProvider.Create(interactableGameAction);

                /*******************************************************************/
                async Task Continue() => await Task.CompletedTask;
                async Task FullLogic(GameAction gameAction) => await logic.Invoke((T)gameAction);
            }
        }

        protected Reaction<T> CreateFinishOptativeReaction<T>(Func<T, bool> condition, Func<T, Task> logic) where T : GameAction
        {
            Reaction<T> newReaction = new(condition, RealLogic);
            _finishReactions.Add(newReaction);
            return newReaction;

            async Task RealLogic(GameAction gameAction)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Optative Reaction");
                interactableGameAction.CreateMainButton().SetLogic(Continue);
                interactableGameAction.Create().SetCard(this).SetInvestigator(Owner).SetLogic(() => FullLogic(gameAction));
                await _gameActionsProvider.Create(interactableGameAction);

                /*******************************************************************/
                async Task Continue() => await Task.CompletedTask;
                async Task FullLogic(GameAction gameAction) => await logic.Invoke((T)gameAction);
            }
        }

        /*******************************************************************/
        protected Activation CreateActivation(Stat activateTurnsCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition, bool isBase = false)
        {
            Activation newActivation = new(activateTurnsCost, logic, condition, isBase);
            _activations.Add(newActivation);
            return newActivation;
        }

        protected Stat CreateStat(int value)
        {
            Stat newSAtat = new(value);
            _stats.Add(newSAtat);
            return newSAtat;
        }

        public bool HasThisStat(Stat stat) => _stats.Contains(stat);
    }
}
