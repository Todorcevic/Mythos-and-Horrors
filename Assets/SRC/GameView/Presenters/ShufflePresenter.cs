using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShufflePresenter : IShuffleAnimator
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        /*******************************************************************/
        public async Task Shuffle(Zone zone)
        {
            await _zoneViewsManager.Get(zone).Shuffle(zone.Cards).AsyncWaitForCompletion();
        }
    }
}
