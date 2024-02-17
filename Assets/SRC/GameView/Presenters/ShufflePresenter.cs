using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShufflePresenter : IPresenter
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is ShuffleGameAction shuffleGameAction)
                await Shuffle(shuffleGameAction);
        }

        /*******************************************************************/
        private async Task Shuffle(ShuffleGameAction shuffleGameAction)
        {
            await _zoneViewsManager.Get(shuffleGameAction.ZoneToShuffle).Shuffle().AsyncWaitForCompletion();
        }
    }
}
