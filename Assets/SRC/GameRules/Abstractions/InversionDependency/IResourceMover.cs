using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IResourceMover
    {
        Task RemoveResource(Investigator card, int amount);

        Task AddResource(Investigator card, int amount);
    }
}
