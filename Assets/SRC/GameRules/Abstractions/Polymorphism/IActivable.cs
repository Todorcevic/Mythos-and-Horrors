using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IActivable
    {
        List<Activation> Activations { get; }
    }
}
