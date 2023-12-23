using System;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        [Inject] public CardInfo Info { get; }
        public Zone OwnZone { get; } = new Zone();
        public Zone CurrentZone { get; private set; }
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
        public bool IsWeakenessCard => Info.Tags.Contains("Weakness") || Info.Tags.Contains("Basic Weakness"); //TODO hacerlo con interface vacia
        public bool IsFaceDown { get; set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException("Zone cant be null");
        }

        public bool CanPlay()
        {
            return true;
        }
    }
}
