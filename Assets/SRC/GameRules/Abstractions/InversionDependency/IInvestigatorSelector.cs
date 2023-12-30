using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{

    public interface IInvestigatorSelector
    {
        Task Select(Investigator investigator);
        Task Select(Zone zone);
    }
}
