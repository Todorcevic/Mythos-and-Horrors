using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card
    {
        [Inject] private readonly CardInfo _info;
        [InjectOptional] private readonly CardExtraInfo _extraInfo;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public Zone OwnZone { get; private set; }
        public List<IBuffable> Buffs { get; private set; } = new();

        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Count > 0;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public List<Effect> PlayableEffects => _effectProvider.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorWithThisCard(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            OwnZone = _zonesProvider.Create();
            FaceDown = new State(false);
            Exausted = new State(false);
        }

        /*******************************************************************/
        public async Task AddBuff(IBuffable newBuff)
        {
            Buffs.Add(newBuff);
            await newBuff.BuffAffectTo(this);
        }

        public async Task RemoveBuff(IBuffable ActivateBuff)
        {
            await ActivateBuff.BuffDeaffectTo(this);
            Buffs.Remove(ActivateBuff);
        }

        public bool HasThisBuff(IBuffable buff) => Buffs.Contains(buff);

        public void TurnDown(bool toFaceDown)
        {
            FaceDown.UpdateValueTo(toFaceDown);
        }
    }
}
