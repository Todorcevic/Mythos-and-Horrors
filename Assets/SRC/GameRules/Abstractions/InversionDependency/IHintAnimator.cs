using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IHintAnimator
    {
        Task GainHints(Investigator investigator, int amount, Card fromCard);
        Task PayHints(Investigator investigator, int amount, Card toCard);
    }
}
