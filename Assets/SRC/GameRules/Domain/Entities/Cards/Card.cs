using System;

namespace GameRules
{
    public class Card
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public CardType Type { get; init; }
        public Zone CurrentZone { get; private set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException(nameof(zone));
        }
    }
}
