using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        private readonly List<IReaction> _reactions = new();

        public List<IReaction> Reactions => _reactions.ToList();

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.FindAll(reaction => reaction.IsAtStart))
                await reaction.React(gameAction);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in _reactions.FindAll(reaction => !reaction.IsAtStart))
                await reaction.React(gameAction);
        }

        /*******************************************************************/
        public IEnumerable<IReaction> FindReactionsByCard(Card card) => _reactions.FindAll(reaction => reaction.Card == card);

        public Reaction<T> FindReactionByLogic<T>(Func<T, Task> logic) where T : GameAction =>
            _reactions.Find(reaction => reaction is Reaction<T> reactionT && reactionT.Logic == logic) as Reaction<T>;

        public void CreateReaction<T>(Card card, Func<T, bool> condition, Func<T, Task> logic, bool isAtStart, bool isBase = false)
            where T : GameAction
        {
            Reaction<T> newReaction = new(card, condition, logic, isAtStart, isBase);
            _reactions.Add(newReaction);
        }

        public void CreateOptativeReaction<T>(Card card, Func<T, bool> condition, Func<T, Task> logic, Investigator investigator, bool isAtStart, bool isBase = false)
            where T : GameAction
        {
            Reaction<T> newReaction = new(card, condition, RealLogic, isAtStart, isBase);
            _reactions.Add(newReaction);

            async Task RealLogic(T gameAction)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Optative Reaction");
                interactableGameAction.CreateMainButton().SetLogic(Continue);
                interactableGameAction.Create().SetCard(card).SetInvestigator(investigator).SetLogic(FullLogic);
                await _gameActionsProvider.Create(interactableGameAction);

                /*******************************************************************/
                async Task Continue() => await Task.CompletedTask;
                async Task FullLogic() => await logic.Invoke(gameAction);
            }
        }

        public void AddRangeReactions(IEnumerable<IReaction> reactions)
        {
            _reactions.AddRange(reactions);
        }

        public void RemoveReaction<T>(Func<T, Task> logic) where T : GameAction =>
            _reactions.RemoveAll(reaction => reaction is Reaction<T> reactionT && reactionT.Logic == logic);

        public void RemoveAllReactionsForThis(IEnumerable<IReaction> reactions)
        {
            foreach (IReaction reaction in reactions)
                _reactions.Remove(reaction);
        }
    }
}
