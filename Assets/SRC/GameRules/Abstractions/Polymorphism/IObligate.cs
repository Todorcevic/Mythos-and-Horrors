using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IObligate
    {
        Zone ZoneToMove { get; }
        Task Obligation();
    }
}
