using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        private readonly List<Effect> _playableEffects = new();
        private readonly List<Buff> _buffs = new();
        [Inject] private readonly CardInfo _info;
        [Inject] protected readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;

        [Inject] public List<History> Histories { get; }
        public virtual CardInfo Info => _info;
        public Zone OwnZone { get; private set; }
        public bool IsFaceDown { get; private set; }
        public IReadOnlyList<Effect> PlayableEffects => _playableEffects;
        public IReadOnlyList<Buff> Buffs => _buffs;
        public bool CanPlay => PlayableEffects.Count > 0;
        public bool HasMultiEffect => PlayableEffects.Count > 1;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorWithThisCard(this);
        public bool IsInHand => CurrentZone == Owner?.HandZone;
        public bool HasOwner => Owner != null;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            OwnZone = _zonesProvider.Create();
        }

        /*******************************************************************/
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

        public async Task AddBuff(Buff newBuff)
        {
            _buffs.Add(newBuff);
            await newBuff.ActivateBuff.Invoke(this);
        }

        public async Task RemoveBuff(Func<Card, Task> ActivateBuff)
        {
            Buff buffToRemove = Buffs.First(buff => buff.ActivateBuff == ActivateBuff);
            await buffToRemove.DeactivateBuff.Invoke(this);
            _buffs.Remove(buffToRemove);
        }

        public bool HasThisBuff(Func<Card, Task> activateBuff) => Buffs.Any(buff => buff.ActivateBuff == activateBuff);

        public void TurnDown(bool toFaceDown)
        {
            IsFaceDown = toFaceDown;
        }
    }
}
