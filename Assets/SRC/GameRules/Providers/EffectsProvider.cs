using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class EffectsProvider
    {
        private readonly List<Effect> _allEffects = new();

        public Effect MainButtonEffect { get; private set; }
        public Effect UniqueEffect => _allEffects.Single();
        public bool IsUniqueEffect => _allEffects.Count() == 1;
        public bool NoEffect => _allEffects.Count() == 0;

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
        public IEnumerable<Effect> GetEffectForThisCard(Card cardAffected) =>
            _allEffects.FindAll(effect => effect.CardAffected == cardAffected);
    }
}
