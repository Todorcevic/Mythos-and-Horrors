using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlaceGoalGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
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
            await _gameActionProvider.Create(new UpdateStatesGameAction(CardGoal.FaceDown, false));
            await _gameActionProvider.Create(new MoveCardsGameAction(CardGoal, _chaptersProviders.CurrentScene.GoalZone));
            await _gameActionProvider.Create(new ShowHistoryGameAction(CardGoal.InitialHistory, CardGoal));
        }
    }
}
