using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IStatAnimator
    {
        Task UpdateStat(Stat stat);
        Task IncreaseStat(Stat stat);
        Task DecreaseStat(Stat stat);
    }
}
