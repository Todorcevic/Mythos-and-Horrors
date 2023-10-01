using System.Collections.Generic;

namespace GameRules
{
    public interface ICardLoader
    {
        void LoadCards(List<Card> cards);
    }
}
