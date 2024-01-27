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
        public Zone OwnZone { get; } = new Zone(ZoneType.Own);
        public Zone CurrentZone { get; private set; }
        public bool IsSpecial => Info.CardType == CardType.None;
        public bool IsScenaryCard => Info.Faction == Faction.Myths;
        public bool IsFaceDown { get; private set; }
        public IReadOnlyList<Effect> PlayableEffects => _playableEffects;
        public bool CanPlay => PlayableEffects.Count > 0;
        public bool HasMultiEffect => PlayableEffects.Count > 1;

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException("Zone cant be null");
        }

        public void AddEffect(Investigator investigator, string description, Func<Task> effect, Investigator investigatorAffected = null)
        {
            _playableEffects.Add(new Effect(investigator, this, description, effect, investigatorAffected));
        }

        public void AddEffect(Effect newEffect)
        {
            _playableEffects.Add(newEffect);
        }

        public void ClearEffects()
        {
            _playableEffects.Clear();
        }

        public void TurnDown(bool isFaceDown)
        {
            IsFaceDown = isFaceDown;
        }
    }
}
