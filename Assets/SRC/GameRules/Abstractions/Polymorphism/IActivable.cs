using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IActivable
    {
        //Stat ActivateTurnsCost { get; }
        List<Activation> Activations { get; }
        //Task Activate();
        //bool ConditionToActivate(Investigator investigator);
    }
}
