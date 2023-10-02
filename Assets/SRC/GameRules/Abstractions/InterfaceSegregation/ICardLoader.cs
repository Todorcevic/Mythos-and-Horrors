using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public interface ICardLoader
    {
        void LoadCards(List<Card> cards);
    }
}
