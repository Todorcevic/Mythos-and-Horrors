using MythosAndHorrors.GameRules.News;
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
        private readonly List<IReaction> _optativeReactions = new();
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
        public void CreateReaction(IReaction realReaction)
        {
            _optativeReactions.Add(realReaction);
        }

        private async Task CreateInteractable(GameAction gameAction, GameActionTime time)
        {
            IEnumerable<IReaction> optativeReactions = _optativeReactions.Where(realReaction => realReaction.Check(gameAction, time)).Except(_played);
            if (!optativeReactions.Any()) return;

            InteractableGameAction interactableGameAction = new(
                   canBackToThisInteractable: true,
                   mustShowInCenter: true,
                   "Play Reaction?", _gameActionsProvider.CurrentInteractable?.ActiveInvestigator ?? _investigatorsProvider.Leader);

            foreach (IReaction reaction in optativeReactions)
            {
                Triggered triggered = (Triggered)reaction;
                interactableGameAction.CreateEffect(
                    card: triggered.Card,
                    activateTurnCost: new Stat(0, false),
                    logic: async () =>
                    {
                        await reaction.React(gameAction);
                        _played.Add(reaction);
                        await CreateInteractable(gameAction, time);
                    },
                    playActionType: triggered.PlayAction,
                    playedBy: triggered.Card.ControlOwner ?? _investigatorsProvider.Leader);
            }

            interactableGameAction.CreateContinueMainButton();
            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
