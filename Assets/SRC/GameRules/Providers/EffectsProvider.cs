using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class EffectsProvider
    {
        private readonly List<Effect> _allEffects = new();

        public List<Effect> AllEffectsPlayable => _allEffects.FindAll(effect => effect.CanPlay.Invoke());
        public Effect UniqueEffect => AllEffectsPlayable.Single();
        public bool IsUniqueEffect => AllEffectsPlayable.Count == 1;
        public bool NoEffect => AllEffectsPlayable.Count == 0;

        /*******************************************************************/
        public void Add(Effect effect) => _allEffects.Add(effect);

        public List<Effect> GetEffectForThisEffectable(IEffectable effectable) =>
            _allEffects.FindAll(effect => effect.Effectable == effectable && effect.CanPlay.Invoke());

        public void ClearAllEffects() => _allEffects.Clear();
    }
}
