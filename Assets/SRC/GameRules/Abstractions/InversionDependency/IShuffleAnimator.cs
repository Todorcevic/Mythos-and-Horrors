using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IShuffleAnimator
    {
        Task Shuffle(Zone zone);
    }
}
