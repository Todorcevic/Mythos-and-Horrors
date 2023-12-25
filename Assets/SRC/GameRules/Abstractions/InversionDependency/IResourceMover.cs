using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IResourceMover
    {
        Task RemoveResource(Adventurer card, int amount);

        Task AddResource(Adventurer card, int amount);
    }
}
