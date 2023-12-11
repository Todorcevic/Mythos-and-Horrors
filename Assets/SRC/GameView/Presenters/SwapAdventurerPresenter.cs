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

        /*******************************************************************/
        public async Task Select(Adventurer adventurer)
        {
            if (_swapAdventurerComponent.AdventurerSelected == adventurer) return;
            await DOTween.Sequence()
                 .Join(_swapAdventurerComponent.Select(adventurer))
                 .Join(_avatarViewsManager.Get(adventurer).Select())
                 .Join(_avatarViewsManager.Get(_swapAdventurerComponent.AdventurerSelected).Deselect())
                 .AsyncWaitForCompletion();
        }

        public async Task Select(Zone zone)
        {
            Adventurer adventurer = _adventurersProvider.GetAdventurerWithThisZone(zone) ?? _swapAdventurerComponent.AdventurerSelected;
            await Select(adventurer);
        }
    }
}
