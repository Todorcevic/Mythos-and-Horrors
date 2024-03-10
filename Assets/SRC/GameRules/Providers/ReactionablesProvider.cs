using System;
using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        [Inject] private readonly DiContainer _diContainer;

        private readonly List<IBuffable> _buffables = new();

        /*******************************************************************/
        public object Create(Type type, object[] args)
        {
            var newReactionable = _diContainer.Instantiate(type, args ?? new object[0]);
            if (newReactionable is IBuffable buffable) _buffables.Add(buffable);
            return newReactionable;
        }

        /*******************************************************************/
        public void CheckActivationBuffs()
        {
            foreach (IBuffable buffable in _buffables)
            {
                buffable.ActivateBuff();
            }
        }

        public void CheckDeactivationBuffs()
        {
            foreach (IBuffable buffable in _buffables)
            {
                buffable.DeactivateBuff();
            }
        }
    }
}
