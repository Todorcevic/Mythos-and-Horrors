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
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
        public bool IsFaceDown { get; set; }
        public int TotalChallengePoints => (Info.Strength ?? 0) + (Info.Agility ?? 0) + (Info.Intelligence ?? 0) + (Info.Power ?? 0) + (Info.Wild ?? 0);
        public int TotalEnemyHits => (Info.EnemyDamage ?? 0) + (Info.EnemyFear ?? 0);


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
