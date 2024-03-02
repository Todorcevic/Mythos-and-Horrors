using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReactionablesProvider
    {
        [Inject] private readonly DiContainer _diContainer;

        private readonly List<IStartReactionable> _startReactionables = new();
        private readonly List<IEndReactionable> _endReactionables = new();
        private readonly List<IBuffable> _buffables = new();

        /*******************************************************************/
        public object Create(Type type, object[] args)
        {
            var newReactionable = _diContainer.Instantiate(type, args ?? new object[0]);
            if (newReactionable is IEndReactionable endReactionable) _endReactionables.Add(endReactionable);
            if (newReactionable is IStartReactionable startReactionable) _startReactionables.Add(startReactionable);
            if (newReactionable is IBuffable buffable) _buffables.Add(buffable);
            return newReactionable;
        }

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IStartReactionable reaction in _startReactionables)
            {
                await reaction.WhenBegin(gameAction);
            }
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IEndReactionable reaction in _endReactionables)
            {
                await reaction.WhenFinish(gameAction);
            }
        }

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
