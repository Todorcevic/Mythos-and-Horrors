using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class PlayInvestigateGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public PlayInvestigateGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigator = investigator;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, Investigator.CurrentPlace.InvestigationTurnsCost.Value));
            await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            await _gameActionsProvider.Create(new InvestigateGameAction(Investigator, Investigator.CurrentPlace));
        }
    }
}
