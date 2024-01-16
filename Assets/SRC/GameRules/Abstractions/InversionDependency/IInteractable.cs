using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IInteractable
    {
        void ActivateAll(List<Card> cards);
        Task DeactivateAll();
    }
}
