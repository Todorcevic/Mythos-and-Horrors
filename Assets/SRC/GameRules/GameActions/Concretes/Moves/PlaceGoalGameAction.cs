using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlaceGoalGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public CardGoal CardGoal { get; }

        /*******************************************************************/
        public PlaceGoalGameAction(CardGoal cardGoal)
        {
            CardGoal = cardGoal;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(CardGoal.FaceDown, false));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CardGoal, _chaptersProviders.CurrentScene.GoalZone));
            await _gameActionsProvider.Create(new ShowHistoryGameAction(CardGoal.InitialHistory, CardGoal));
        }
    }
}
