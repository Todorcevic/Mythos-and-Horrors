using System;
using Zenject;

namespace GameRules
{
    public class Card
    {
        [Inject] public CardInfo Info { get; }
        public Zone CurrentZone { get; private set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException(nameof(zone));
        }
    }
}
