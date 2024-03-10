using System.Collections.Generic;
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
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        public State FaceDown { get; private set; }
        public State Exausted { get; private set; }
        public Zone OwnZone { get; private set; }
        public List<IBuffable> Buffs { get; private set; } = new();
        public List<Buff> Buuffs => _buffsProvider.GetBuffsForThisCard(this);

        /*******************************************************************/
        public virtual CardInfo Info => _info;
        public CardExtraInfo ExtraInfo => _extraInfo;
        public bool CanBePlayed => PlayableEffects.Count > 0;
        public Zone CurrentZone => _zonesProvider.GetZoneWithThisCard(this);
        public List<Effect> PlayableEffects => _effectProvider.GetEffectForThisCard(this);
        public Investigator Owner => _investigatorsProvider.GetInvestigatorWithThisCard(this);

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            OwnZone = _zonesProvider.Create();
            FaceDown = new State(false);
            Exausted = new State(false);

            _reactionablesProvider.SubscribeAtStart(WhenBegin);
            _reactionablesProvider.SubscribeAtEnd(WhenFinish);
        }

        protected virtual Task WhenBegin(GameAction gameAction) => Task.CompletedTask;

        protected virtual Task WhenFinish(GameAction gameAction) => Task.CompletedTask;

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



        List<Buff> newBuuffs = new();

        public async Task ActivateBuff(Buff newBuff)
        {
            newBuuffs.Add(newBuff);
            await newBuff.ActivationLogic(this);
        }

        public async Task DeactivateBuff(Buff ActivateBuff)
        {
            await ActivateBuff.ActivationLogic(this);
            newBuuffs.Remove(ActivateBuff);
        }
    }
}
