using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class EffectsProvider
    {
        private readonly List<Effect> _allEffects = new();

        /*******************************************************************/
        public void Add(Effect effect) => _allEffects.Add(effect);

        public List<Effect> GetEffectForThisCard(Card card) => _allEffects.FindAll(effect => effect.Card == card && effect.CanPlay.Result);

        public Effect GetEffectWithThiLogic(Func<Task> logic) => _allEffects.Find(effect => effect.Logic == logic);

        public void ClearAllEffects() => _allEffects.Clear();
    }
}
