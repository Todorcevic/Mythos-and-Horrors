using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class AnimatorsProvider
    {
        [Inject] private readonly List<IAnimator> _animators;

        /*******************************************************************/
        public async Task LaunchAnimation(GameAction gameAction)
        {
            foreach (IAnimator animator in _animators)
            {
                await animator.Checking(gameAction);
            }
        }
    }
}
