using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        private readonly List<Effect> _playableEffects = new();

        [Inject] protected readonly GameActionFactory _gameActionFactory;
        [Inject] public CardInfo Info { get; }
        [Inject] public List<History> Histories { get; }
        public Zone OwnZone { get; } = new Zone();
        public Zone CurrentZone { get; private set; }
        public bool IsSpecial => Info.CardType == CardType.None;
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
        public bool IsFaceDown { get; set; }
        public IReadOnlyList<Effect> PlayableEffects => _playableEffects;
        public bool CanPlay => PlayableEffects.Count > 0;

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException("Zone cant be null");
        }

        public void AddEffect(string description, Func<Task> effect)
        {
            _playableEffects.Add(new Effect(this, description, effect));
        }

        public void ClearEffects()
        {
            _playableEffects.Clear();
        }
    }
}
