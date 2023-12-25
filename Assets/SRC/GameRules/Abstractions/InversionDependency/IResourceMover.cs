using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IResourceMover
    {
        Task RemoveResource(Card card, int amount);

        Task AddResource(Card card, int amount);
    }
}
