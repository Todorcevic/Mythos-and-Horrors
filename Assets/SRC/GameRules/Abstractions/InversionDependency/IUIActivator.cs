using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public interface IUIActivator
    {
        void Activate(List<Card> cards);
        void Deactivate();
    }
}
