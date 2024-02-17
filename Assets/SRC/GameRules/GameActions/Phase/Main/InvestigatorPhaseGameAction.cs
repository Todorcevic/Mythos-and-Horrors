using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class InvestigatorPhaseGameAction : GameAction, IPhase
    {
        string IPhase.Name => throw new System.NotImplementedException();
        string IPhase.Description => throw new System.NotImplementedException();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Task.CompletedTask;
        }
    }
}
