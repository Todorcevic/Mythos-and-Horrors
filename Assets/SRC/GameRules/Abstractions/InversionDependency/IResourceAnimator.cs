using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IResourceAnimator
    {
        Task GainResource(Investigator investigator, int amount, Stat fromCard);
        Task PayResource(Investigator investigator, int amount, Stat toCard);
    }
}
