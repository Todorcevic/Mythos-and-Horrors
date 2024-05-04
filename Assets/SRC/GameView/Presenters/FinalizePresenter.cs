using MythosAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameView
{
    public class FinalizePresenter : IPresenter<FinalizeGameAction>
    {
        public async Task PlayAnimationWith(FinalizeGameAction gameAction)
        {
            await Task.CompletedTask;
        }
    }
}
