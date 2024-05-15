using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayTakeResourceGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public PlayTakeResourceGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, Investigator.TurnsCost.Value));
            await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            await _gameActionsProvider.Create(new GainResourceGameAction(Investigator, 1));
        }
    }
}
