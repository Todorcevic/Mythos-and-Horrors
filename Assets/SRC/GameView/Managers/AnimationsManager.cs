using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class AnimationsManager
    {
        private readonly Dictionary<string, PlayAnimationSO> _allAnimations;

        /*******************************************************************/
        public AnimationsManager(List<PlayAnimationSO> allAnimations)
        {
            _allAnimations = allAnimations.ToDictionary(playAnimation => playAnimation.LocalizableCode);
        }

        /*******************************************************************/
        public PlayAnimationSO GetAnimation(string localizableCode)
        {
            return _allAnimations[localizableCode];
        }
    }
}
