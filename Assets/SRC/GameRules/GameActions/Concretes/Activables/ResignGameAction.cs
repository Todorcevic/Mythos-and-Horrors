using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class ResignGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public ResignGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new EliminateInvestigatorGameAction(Investigator));
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Investigator.Resign, IsActive));
        }
    }
}
