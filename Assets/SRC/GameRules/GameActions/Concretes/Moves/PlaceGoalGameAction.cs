using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlaceGoalGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public CardGoal CardGoal { get; private set; }
        public override bool CanBeExecuted => CardGoal != null;

        /*******************************************************************/
        public PlaceGoalGameAction SetWith(CardGoal cardGoal)
        {
            CardGoal = cardGoal;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(CardGoal.FaceDown, false).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CardGoal, _chaptersProviders.CurrentScene.GoalZone).Execute();
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(CardGoal.InitialHistory, CardGoal).Execute();
        }
    }
}
