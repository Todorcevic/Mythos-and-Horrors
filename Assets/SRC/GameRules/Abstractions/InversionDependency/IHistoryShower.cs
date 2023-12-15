using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IHistoryShower
    {
        Task ShowHistoryAsync(History history);
    }
}
