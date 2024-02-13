using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class ChangePhasePresenter : IPresenter
    {
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is IPhase phase)
                await ChangePhaseWith(phase);
        }

        /*******************************************************************/
        private async Task ChangePhaseWith(IPhase phase)
        {
            //TODO: Implementar visor de cambio de fase

            await Task.CompletedTask;
        }
    }
}
