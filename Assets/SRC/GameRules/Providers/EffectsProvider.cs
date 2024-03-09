using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EffectsProvider
    {
        [Inject] private readonly DiContainer _diContainer;
        private readonly List<Effect> _allEffects = new();

        public Effect MainButtonEffect { get; private set; }
        public List<Effect> AllEffectsPlayable => _allEffects.FindAll(effect => effect.CanPlay.Invoke());
        public Effect UniqueEffect => AllEffectsPlayable.Single();
        public bool IsUniqueEffect => AllEffectsPlayable.Count == 1;
        public bool NoEffect => AllEffectsPlayable.Count == 0;

        /*******************************************************************/
        public Effect Create()
        {
            Effect effect = _diContainer.Instantiate<Effect>();
            _allEffects.Add(effect);
            return effect;
        }

        public Effect CreateMainButton()
        {
            Effect effect = _diContainer.Instantiate<Effect>();
            MainButtonEffect = effect;
            return effect;
        }

        public void ClearAllEffects()
        {
            _allEffects.Clear();
            MainButtonEffect = null;
        }

        /*******************************************************************/
        public List<Effect> GetEffectForThisCard(Card cardAffected) =>
            _allEffects.FindAll(effect => effect.CardAffected == cardAffected && effect.CanPlay.Invoke());

        public Effect GetSpecificEffect(Func<Task> logic) => _allEffects.Find(effect => effect.Logic == logic);
    }
}
