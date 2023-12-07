using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapAdventurerPresenter : IAdventurerSelector
    {
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [Inject] private readonly IUAActivator _activatorUIPresenter;

        /*******************************************************************/
        public async Task Select(Adventurer adventurer)
        {
            if (_swapAdventurerComponent.AdventurerSelected == adventurer) return;
            await DOTween.Sequence()
                  .OnStart(() => _activatorUIPresenter.DeactivateSensor())
                 .Join(_swapAdventurerComponent.Select(adventurer))
                 .Join(_avatarViewsManager.Get(adventurer).Select())
                 .Join(_avatarViewsManager.Get(_swapAdventurerComponent.AdventurerSelected).Deselect())
                 .OnComplete(() => _activatorUIPresenter.ActivateSensor())
                 .AsyncWaitForCompletion();
        }

        public async Task Select(Zone zone)
        {
            Adventurer adventurer = _adventurersProvider.GetAdventurerWithThisZone(zone);
            await Select(adventurer);
        }
    }
}
