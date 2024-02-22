using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShufflePresenter2 : INewPresenter<ShuffleGameAction>
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        /*******************************************************************/
        async Task INewPresenter<ShuffleGameAction>.CheckGameAction(ShuffleGameAction shuffleGameAction)
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
