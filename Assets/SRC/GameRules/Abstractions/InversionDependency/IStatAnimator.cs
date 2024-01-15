using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IStatAnimator
    {
        Task UpdateStat(Stat stat);
    }
}
