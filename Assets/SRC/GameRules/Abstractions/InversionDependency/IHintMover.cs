using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IHintMover
    {
        Task GainHints(Investigator investigator, Card fromCard, int amount);

        Task PayHints(Investigator investigator, Card toCard, int amount);
    }
}
