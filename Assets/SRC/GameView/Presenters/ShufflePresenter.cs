using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShufflePresenter : IPresenter<ShuffleGameAction>
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        /*******************************************************************/
        async Task IPresenter<ShuffleGameAction>.PlayAnimationWith(ShuffleGameAction shuffleGameAction)
        {
            await Shuffle(shuffleGameAction);
        }

        /*******************************************************************/
        private async Task Shuffle(ShuffleGameAction shuffleGameAction)
        {
            await _zoneViewsManager.Get(shuffleGameAction.ZoneToShuffle).Shuffle().AsyncWaitForCompletion();
        }
    }
}
