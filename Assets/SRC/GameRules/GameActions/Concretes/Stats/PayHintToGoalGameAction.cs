using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintToGoalGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; }
        public int Amount { get; }

        /*******************************************************************/
        public PayHintToGoalGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            Amount = amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionProvider.Create(new PayHintGameAction(Investigator, _chaptersProvider.CurrentScene.CurrentGoal.Hints, Amount));
            await _gameActionProvider.Create(new CheckHintsGoalGameAction());
        }
    }
}
