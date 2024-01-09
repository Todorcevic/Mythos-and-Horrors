using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IResourceMover
    {
        Task PayResource(Investigator investigator, int amount, Card toCard);

        Task GainResource(Investigator investigator, int amount, Card fromCard);
    }
}
