using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules.News
{
    public class OptativeReaction<T> : Triggered, IReaction where T : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Card Card { get; }
        public GameConditionWith<T> Condition { get; }
        public GameCommand<T> Command { get; }
        public GameActionTime Time { get; }
        public bool IsDisable { get; private set; }


        /*******************************************************************/
        public OptativeReaction(Card card, GameConditionWith<T> condition, GameCommand<T> logic, PlayActionType playActionType, GameActionTime time)
            : base(playActionType)
        {
            Card = card;
            Condition = condition;
            Command = logic;
            Time = time;
        }

        /*******************************************************************/
        public async Task React(GameAction gameAction)
        {
            if (IsDisable) return;
            if (gameAction.IsCancel) return;
            if (gameAction is not T realGameAction) return;
            if (!Condition.IsTrueWith(realGameAction)) return;
            await RunWith(realGameAction);
        }

        private async Task RunWith(T gameAction)
        {
            InteractableGameAction interactableGameAction = new(
                   canBackToThisInteractable: true,
                   mustShowInCenter: true,
                   "Play Reaction?",
                   Card.ControlOwner ?? _investigatorsProvider.Leader);
            interactableGameAction.CreateContinueMainButton();
            interactableGameAction.CreateEffect(
                card: Card,
                activateTurnCost: new Stat(0, false),
                logic: () => Command.RunWith(gameAction),
                playActionType: PlayAction,
                playedBy: Card.ControlOwner ?? _investigatorsProvider.Leader);
            await _gameActionsProvider.Create(interactableGameAction);
        }

        public void Disable() => IsDisable = true;

        public void Enable() => IsDisable = false;
    }
}
