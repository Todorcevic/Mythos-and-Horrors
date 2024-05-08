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

        private IEnumerable<Card> TomesInPlay => Owner.CardsInPlay.Where(card => card.Tags.Contains(Tag.Tome));

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = new State(false);
            CreateActivation(CreateStat(0), FreeTomeActivationActivate, FreeTomeActivationConditionToActivate);
            _reactionablesProvider.CreateReaction<RoundGameAction>(this, RestartAbilityCondition, RestartAbilityLogic, true);
        }

        /*******************************************************************/
        public async Task FreeTomeActivationActivate(Investigator activeInvestigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Tome");

            foreach (Card activable in _cardsProvider.AllCards.Where(card => card.Tags.Contains(Tag.Tome) && card.IsInPlay && card.IsActivable))
            {
                foreach (Activation activation in activable.AllActivations)
                {
                    if (activation.Condition(activeInvestigator))
                        interactableGameAction.Create()
                        .SetCard(activable)
                        .SetInvestigator(activeInvestigator)
                        .SetLogic(Activate);

                    /*******************************************************************/
                    async Task Activate()
                    {
                        int realTurnsCost = activation.ActivateTurnsCost.Value;
                        await _gameActionsProvider.Create(new DecrementStatGameAction(activation.ActivateTurnsCost, 1));
                        await _gameActionsProvider.Create(new ActivateCardGameAction(activation, activeInvestigator));
                        await _gameActionsProvider.Create(new UpdateStatGameAction(activation.ActivateTurnsCost, realTurnsCost));
                        await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, true));
                    }
                }
            }
            await _gameActionsProvider.Create(interactableGameAction);
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
            await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, false));
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

        private async Task DrawCards() => await _gameActionsProvider.Create(new SafeForeach<Card>(TomesInPlay, DrawAid));

        private async Task DrawAid(Card tome) => await _gameActionsProvider.Create(new DrawAidGameAction(Owner));
    }
}
