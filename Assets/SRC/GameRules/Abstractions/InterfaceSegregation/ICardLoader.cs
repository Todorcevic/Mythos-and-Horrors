using System.Collections.Generic;

namespace Tuesday.GameRules
{
    public interface ICardLoader
    {
        void LoadCards(List<Card> cards);
    }
}
