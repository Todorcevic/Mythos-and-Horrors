using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class EffectsProvider
    {
        private readonly List<Effect> _allEffects = new();

        /*******************************************************************/
        public void Add(Effect effect) => _allEffects.Add(effect);

        public List<Effect> GetEffectForThisCard(Card card) => _allEffects.FindAll(effect => effect.Card == card);

        public void ClearAllEffects() => _allEffects.Clear();
    }
}
