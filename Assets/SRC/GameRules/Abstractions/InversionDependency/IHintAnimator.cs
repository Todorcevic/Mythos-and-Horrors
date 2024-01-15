using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IHintAnimator
    {
        Task GainHints(Investigator investigator, int amount, Stat fromCard);
        Task PayHints(Investigator investigator, int amount, Stat toCard);
    }
}
