using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class ResourceMoverPresenter : IResourceMover
    {
        [Inject] private readonly CardViewsManager _cardsManager;

        /*******************************************************************/
        public async Task AddResource(Adventurer card, int amount)
        {
            await Task.CompletedTask;
        }

        public async Task RemoveResource(Adventurer card, int amount)
        {
            await Task.CompletedTask;
        }
    }
}
