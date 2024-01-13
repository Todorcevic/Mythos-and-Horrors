using System;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        [Inject] public CardInfo Info { get; }
        [Inject] public List<History> Histories { get; }
        public Zone OwnZone { get; } = new Zone();
        public Zone CurrentZone { get; private set; }
        public bool IsSpecial => Info.CardType == CardType.None;
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
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

        public void Play()
        {
            if (!CanPlay()) throw new InvalidOperationException("Card cant be played");
        }
    }
}
