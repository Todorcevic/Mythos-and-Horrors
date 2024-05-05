using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class FinalizePresenter : IPresenter<FinalizeGameAction>
    {
        [Inject] private readonly RegisterChapterComponent _registerChapterComponent;

        /*******************************************************************/
        public async Task PlayAnimationWith(FinalizeGameAction gameAcfinalizeGameActiontion)
        {
            await _registerChapterComponent.Show();
        }
    }
}
