using System;
using UnityEngine;

namespace GameRules
{
    public class Card
    {
        [SerializeField] public CardType Type { get; init; }
        public CardInfo Info { get; init; }
        public Zone CurrentZone { get; private set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException(nameof(zone));
        }
    }
}
