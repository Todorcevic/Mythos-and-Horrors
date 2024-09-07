using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OptativeReactionsProvider
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        private readonly List<IReaction> _optativeCodes = new();
        private readonly List<IReaction> _played = new();

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            _played.Clear();
            await CreateInteractable(gameAction, GameActionTime.Before);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            _played.Clear();
            await CreateInteractable(gameAction, GameActionTime.After);
        }

        /*******************************************************************/
        public OptativeReaction<T> CreateOptativeReaction<T>(Card card, Func<T, bool> condition, Func<T, Task> logic, GameActionTime time, Localization localization,
            PlayActionType playActionType = PlayActionType.None) where T : GameAction
        {
            OptativeReaction<T> optativeReaction = new(card, new GameConditionWith<T>(condition), new GameCommand<T>(logic), playActionType, time, localization);
            _optativeCodes.Add(optativeReaction);
            return optativeReaction;
        }

        /*******************************************************************/
        private async Task CreateInteractable(GameAction gameAction, GameActionTime time)
        {
            IEnumerable<IReaction> optativeReactions = _optativeCodes.Where(realReaction => realReaction.Check(gameAction, time)).Except(_played);
            if (!optativeReactions.Any()) return;

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, new Localization("Interactable_OptativeReactions"));

            foreach (IReaction reaction in optativeReactions)
            {
                ITriggered triggered = (ITriggered)reaction;

                interactableGameAction.CreateCardEffect(
                    card: triggered.Card,
                    activateTurnCost: new Stat(0, false),
                    logic: async () =>
                    {
                        await reaction.React(gameAction);
                        _played.Add(reaction);
                        await CreateInteractable(gameAction, time);
                    },
                    playActionType: triggered.PlayAction,
                    playedBy: triggered.Card.ControlOwner ?? _investigatorsProvider.Leader,
                    triggered.Localization,
                    resourceCost: GetResourceCostFor(triggered));
            }

            interactableGameAction.CreateContinueMainButton();
            await interactableGameAction.Execute();
        }

        private Stat GetResourceCostFor(ITriggered triggered)
        {
            if (triggered.Card is CardConditionReaction cardConditionFast) return cardConditionFast.ResourceCost;
            return null;
        }
    }
}
