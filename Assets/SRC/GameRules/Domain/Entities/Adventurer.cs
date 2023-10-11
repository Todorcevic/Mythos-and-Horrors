using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class Adventurer
    {
        public Card AdventurerCardCode { get; set; }
        public List<Card> CardsCode { get; set; }
        public List<Card> RequerimentCardCodes { get; set; }
        public Dictionary<Faction, int> DeckBuildingConditions { get; set; }
        public int DeckSize { get; set; }
        public int Xp { get; set; }
        public int Injury { get; set; }
        public int Shock { get; set; }
    }
}
