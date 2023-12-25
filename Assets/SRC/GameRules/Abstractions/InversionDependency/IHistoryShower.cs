using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IHistoryShower
    {
        Task ShowHistory(History history);
    }
}
