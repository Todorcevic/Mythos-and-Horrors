using System;

namespace GameRules
{
    public class Card
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public CardType Type { get; private set; }
        public Zone CurrentZone { get; private set; }

        /*******************************************************************/
        public Card(string id, string name, CardType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException(nameof(zone));
        }
    }
}
