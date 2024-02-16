using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IShowHistory
    {
        Task Show(History history);
    }
}
