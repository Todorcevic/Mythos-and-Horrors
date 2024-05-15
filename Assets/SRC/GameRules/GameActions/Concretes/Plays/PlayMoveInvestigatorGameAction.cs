using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayMoveInvestigatorGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public PlayMoveInvestigatorGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigator = investigator;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, CardPlace.MoveTurnsCost.Value));
            await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(Investigator, CardPlace));
        }
    }
}
