using System;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        [Inject] public CardInfo Info { get; }
        public Zone OwnZone { get; } = new Zone();
        public Zone CurrentZone { get; private set; }
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
        public bool IsFaceDown { get; set; }
        public int Resources { get; private set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException("Zone cant be null");
        }

        public bool CanPlay()
        {
            return true;
        }

        public void RemoveResources(int amount)
        {
            if (amount < 0) throw new ArgumentException("Amount cant be negative");
            Resources -= amount;
        }
    }
}
