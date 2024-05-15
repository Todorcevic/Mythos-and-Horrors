using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayConfronGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardCreature CardCreature { get; }

        /*******************************************************************/
        public PlayConfronGameAction(Investigator investigator, CardCreature cardCreature)
        {
            Investigator = investigator;
            CardCreature = cardCreature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, CardCreature.InvestigatorConfronTurnsCost.Value));
            await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            await _gameActionsProvider.Create(new MoveCardsGameAction(CardCreature, Investigator.DangerZone));
        }
    }
}
