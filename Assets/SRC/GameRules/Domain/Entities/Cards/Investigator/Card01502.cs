using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01502 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public State AbilityUsed { get; private set; }

        private IEnumerable<Card> TomesInPlay() => Owner.CardsInPlay.Where(card => card.Tags.Contains(Tag.Tome));
        public override IEnumerable<Tag> Tags => new[] { Tag.Miskatonic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = CreateState(false);
            CreateFastActivation(FreeTomeActivationActivate, FreeTomeActivationConditionToActivate, PlayActionType.Activate);
            CreateForceReaction<RoundGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        public async Task FreeTomeActivationActivate(Investigator activeInvestigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Tome");
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Owner.CurrentTurns, 1).Execute();

            foreach (Card activable in _cardsProvider.AllCards.Where(card => card.Tags.Contains(Tag.Tome) && card.IsInPlay && card.IsActivable))
            {
                foreach (Activation<Investigator> activation in activable.AllActivations.Where(activation => !activation.IsFreeActivation))
                {
                    if (activation.Condition.IsTrueWith(activeInvestigator))
                        interactableGameAction.CreateEffect(activable, activation.ActivateTurnsCost, Activate, PlayActionType.Choose | activation.PlayAction, playedBy: activeInvestigator);

                    /*******************************************************************/
                    async Task Activate()
                    {
                        await activation.PlayFor(activeInvestigator);
                        await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Execute();
                    }
                }
            }
            await interactableGameAction.Execute();
        }

        public bool FreeTomeActivationConditionToActivate(Investigator activeInvestigator)
        {
            if (AbilityUsed.IsActive) return false;
            if (!IsInPlay) return false;
            if (Owner != activeInvestigator) return false;
            if (!_cardsProvider.AllCards.Where(card => card.Tags.Contains(Tag.Tome) && card.IsInPlay && card.IsActivable).Any()) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RestartAbilityLogic(RoundGameAction gameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Execute();
        }

        private bool RestartAbilityCondition(RoundGameAction gameAction)
        {
            if (gameAction is not RoundGameAction) return false;
            if (!AbilityUsed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task StarEffect()
        {
            _gameActionsProvider.CurrentChallenge.SuccesEffects.Add(DrawCards);
            await Task.CompletedTask;
        }

        protected override int StarValue() => 0;

        private async Task DrawCards() => await _gameActionsProvider.Create<SafeForeach<Card>>().SetWith(TomesInPlay, DrawAid).Execute();

        private async Task DrawAid(Card tome) => await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(Owner).Execute();
    }
}
