using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class AnimatorsProvider
    {
        [Inject] private readonly List<IAnimatorStart> _startAnimators;
        [Inject] private readonly List<IAnimatorEnd> _endAnimators;

        /*******************************************************************/
        public async Task LaunchStartAnimation(GameAction gameAction)
        {
            foreach (IAnimatorStart animator in _startAnimators)
            {
                await animator.CheckingAtStart(gameAction);
            }
        }

        public async Task LaunchEndAnimation(GameAction gameAction)
        {
            foreach (IAnimatorEnd animator in _endAnimators)
            {
                await animator.CheckingAtEnd(gameAction);
            }
        }
    }
}
