using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class EffectsProvider
    {
        private readonly List<Effect> _allEffects = new();

        public Effect MainButtonEffect { get; private set; }
        public List<Effect> AllEffectsPlayable => _allEffects;
        public Effect UniqueEffect => AllEffectsPlayable.Single();
        public bool IsUniqueEffect => AllEffectsPlayable.Count == 1;
        public bool NoEffect => AllEffectsPlayable.Count == 0;

        /*******************************************************************/
        public Effect Create()
        {
            Effect effect = new();
            _allEffects.Add(effect);
            return effect;
        }

        public Effect CreateMainButton()
        {
            Effect effect = new();
            MainButtonEffect = effect;
            return effect;
        }

        public void RemoveEffect(Effect effect) => _allEffects.Remove(effect);

        public void ClearAllEffects()
        {
            _allEffects.Clear();
            MainButtonEffect = null;
        }

        /*******************************************************************/
        public List<Effect> GetEffectForThisCard(Card cardAffected) =>
            _allEffects.FindAll(effect => effect.CardAffected == cardAffected);
    }
}
