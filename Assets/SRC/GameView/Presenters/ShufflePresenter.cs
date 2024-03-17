using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShufflePresenter : IPresenter<ShuffleGameAction>
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;

        /*******************************************************************/
        async Task IPresenter<ShuffleGameAction>.PlayAnimationWith(ShuffleGameAction shuffleGameAction)
        {
            await Shuffle(shuffleGameAction);
        }

        /*******************************************************************/
        private async Task Shuffle(ShuffleGameAction shuffleGameAction)
        {
            await _swapInvestigatorPresenter.Select(shuffleGameAction.ZoneToShuffle).AsyncWaitForCompletion();
            await _zoneViewsManager.Get(shuffleGameAction.ZoneToShuffle).Shuffle().AsyncWaitForCompletion();
        }

        /*******************************************************************/
        Task IPresenter<ShuffleGameAction>.UndoAnimationWith(ShuffleGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
