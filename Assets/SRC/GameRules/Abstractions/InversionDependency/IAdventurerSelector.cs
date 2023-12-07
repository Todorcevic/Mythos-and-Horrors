using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IAdventurerSelector
    {
        Task Select(Adventurer adventurer);
        Task Select(Zone zone);
    }
}
