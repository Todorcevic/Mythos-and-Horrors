using System;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        public CardInfo Info { get; private set; }
        public Zone OwnZone { get; } = new Zone();
        public Zone CurrentZone { get; private set; }
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
        public bool IsFaceDown { get; set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(CardInfo cardInfo)
        {
            Info = cardInfo ?? throw new ArgumentNullException("CardInfo cant be null");
        }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException("Zone cant be null");
        }
    }
}
