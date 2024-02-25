using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card
    {
        private readonly List<IBuffable> _buffs = new();
        [Inject] private readonly CardInfo _info;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        public virtual CardInfo Info => _info;
        public Zone OwnZone { get; private set; }
        public bool IsFaceDown { get; private set; }
        public IReadOnlyList<Effect> PlayableEffects => _effectProvider.GetEffectForThisCard(this);
        public IReadOnlyList<IBuffable> Buffs => _buffs;
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
            _effectProvider.Add(new Effect(this, investigator, description, effect, investigatorAffected));
        }

        public async Task AddBuff(IBuffable newBuff)
        {
            _buffs.Add(newBuff);
            await newBuff.BuffAffectTo(this);
        }

        public async Task RemoveBuff(IBuffable ActivateBuff)
        {
            await ActivateBuff.BuffDeaffectTo(this);
            _buffs.Remove(ActivateBuff);
        }

        public bool HasThisBuff(IBuffable buff) => _buffs.Contains(buff);

        public void TurnDown(bool toFaceDown)
        {
            IsFaceDown = toFaceDown;
        }
    }
}
