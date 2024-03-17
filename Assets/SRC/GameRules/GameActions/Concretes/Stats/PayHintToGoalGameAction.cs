using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintToGoalGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
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
            await _gameActionsProvider.Create(new PayHintGameAction(Investigator, _chaptersProvider.CurrentScene.CurrentGoal.Hints, Amount));
            await _gameActionsProvider.Create(new CheckHintsGoalGameAction());
        }
    }
}
