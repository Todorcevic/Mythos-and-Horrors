using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayDrawCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public PlayDrawCardGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, Investigator.DrawTurnsCost.Value));
            await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            await _gameActionsProvider.Create(new DrawAidGameAction(Investigator));
        }
    }
}
