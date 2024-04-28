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

        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public Zone OwnZone { get; private set; }
        public IEnumerable<Buff> Buffs => _buffsProvider.GetBuffsForThisCard(this);

        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public virtual IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Count() > 0;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IEnumerable<Effect> PlayableEffects => _gameActionsProvider.CurrentInteractable?.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorOwnerWithThisZone(CurrentZone) ??
            _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public bool IsInPlay => ZoneType.PlayZone.HasFlag(CurrentZone.ZoneType);

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

        protected virtual Task WhenBegin(GameAction gameAction) => Task.CompletedTask;

        protected virtual Task WhenFinish(GameAction gameAction) => Task.CompletedTask;

        /*******************************************************************/
        protected async Task Reaction<T>(GameAction gameAction, Func<T, bool> condition, Func<T, Task> logic) where T : GameAction
        {
            if (gameAction is not T realGameAction) return;
            if (!condition.Invoke(realGameAction)) return;

            await logic.Invoke(realGameAction);
        }

        protected async Task OptativeReaction<T>(GameAction gameAction, Func<T, bool> condition, Func<T, Task> logic)
            where T : GameAction
        {
            if (gameAction is not T realGameAction) return;
            if (!condition.Invoke(realGameAction)) return;

            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Optative Reaction");
            interactableGameAction.CreateMainButton().SetLogic(Continue);
            interactableGameAction.Create().SetCard(this).SetInvestigator(Owner).SetLogic(FullLogic);
            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task Continue() => await Task.CompletedTask;
            async Task FullLogic() => await logic.Invoke(realGameAction);
        }
    }
}
