using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class OptativeReaction
    {
        private readonly GameActionsProvider _gameActionsProvider;
        public Card Card { get; }
        public Func<GameAction, bool> Condition { get; }
        public Func<GameAction, Task> Logic { get; }

        /*******************************************************************/
        public OptativeReaction(Card card, Func<GameAction, bool> condition, Func<GameAction, Task> logic, GameActionsProvider gameActionsProvider)
        {
            Card = card;
            Condition = condition;
            Logic = logic;
            _gameActionsProvider = gameActionsProvider;
        }

        /*******************************************************************/
        public async Task React(GameAction gameAction)
        {
            if (!Condition.Invoke(gameAction)) return;

            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Optative Reaction");
            interactableGameAction.CreateMainButton().SetLogic(Continue);
            interactableGameAction.Create().SetCard(Card).SetInvestigator(Card.Owner).SetLogic(FullLogic);
            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task Continue() => await Task.CompletedTask;
            async Task FullLogic() => await Logic.Invoke(gameAction);
        }
    }
}
