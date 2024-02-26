using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card : IEffectable
    {
        private readonly List<IBuffable> _buffs = new();
        [Inject] private readonly CardInfo _info;
        [InjectOptional] private readonly CardExtraInfo _extraInfo;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        public virtual CardInfo Info => _info;
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanPlay => PlayableEffects.Count > 0;
        public bool HasMultiEffect => PlayableEffects.Count > 1;
        public bool IsInHand => CurrentZone == Owner?.HandZone;
        public bool HasOwner => Owner != null;
        public State FaceDown { get; private set; }
        public Zone OwnZone { get; private set; }
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public IReadOnlyList<Effect> PlayableEffects => _effectProvider.GetEffectForThisEffectable(this);
        public IReadOnlyList<IBuffable> Buffs => _buffs;
        public Investigator Owner => _investigatorsProvider.GetInvestigatorWithThisCard(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            OwnZone = _zonesProvider.Create();
            FaceDown = new State(false);
        }

        /*******************************************************************/
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
            FaceDown.UpdateValue(toFaceDown);
        }
    }
}
