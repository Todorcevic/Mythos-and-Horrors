using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public interface IUIActivable
    {
        void ActivateAll(List<Card> cards);
        void DeactivateAll();
    }
}
