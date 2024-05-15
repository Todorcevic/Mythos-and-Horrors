using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayEludeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardCreature CardCreature { get; }

        /*******************************************************************/
        public PlayEludeGameAction(Investigator investigator, CardCreature cardCreature)
        {
            Investigator = investigator;
            CardCreature = cardCreature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, CardCreature.InvestigatorConfronTurnsCost.Value));
            await _gameActionsProvider.Create(new EludeGameAction(Investigator, CardCreature));
        }
    }
}
