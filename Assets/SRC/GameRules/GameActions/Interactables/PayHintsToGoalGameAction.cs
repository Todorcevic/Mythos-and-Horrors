using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintsToGoalGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IAsGroupPresenter _asGroupPresenter;

        public bool ButtonCanUndo => _gameActionsProvider.CanUndo(realLast: true);
        public CardGoal CardGoal { get; }
        public IEnumerable<Investigator> InvestigatorsToPay { get; }
        public override bool CanBeExecuted => !CardGoal.Revealed.IsActive && CardGoal.Hints.Value > 0;

        /*******************************************************************/
        public PayHintsToGoalGameAction(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay)
        {
            CardGoal = cardGoal;
            InvestigatorsToPay = investigatorsToPay;
        }
        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Card, int> result = await _asGroupPresenter.SelectWith(this);
            if (result == null) await UndoLogic();
            await _gameActionsProvider.Create(new SafeForeach<CardAvatar>(() => result.Keys.OfType<CardAvatar>(), Logic));

            /*******************************************************************/
            async Task Logic(CardAvatar AvatarCard) =>
                await _gameActionsProvider.Create(new PayHintGameAction(AvatarCard.Owner, CardGoal.Hints, result[AvatarCard]));
        }


        private async Task UndoLogic()
        {
            InteractableGameAction lastInteractable = await _gameActionsProvider.CancelInteractable();
            if (lastInteractable.GetType() != typeof(InteractableGameAction)) lastInteractable.ClearEffects();
            await _gameActionsProvider.Create(lastInteractable);
        }

    }
}
