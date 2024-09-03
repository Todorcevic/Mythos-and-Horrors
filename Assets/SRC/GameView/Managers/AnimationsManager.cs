using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AnimationsManager
    {
        private readonly Dictionary<string, PlayAnimationSO> _allAnimations;

        /*******************************************************************/
        public AnimationsManager(List<PlayAnimationSO> allAnimations)
        {
            _allAnimations = allAnimations.ToDictionary(playAnimation => playAnimation.name);
        }

        /*******************************************************************/
        public AudioClip GetAnimation(CardEffect cardEffect)
        {
            _allAnimations.TryGetValue(cardEffect.CardOwner.Info.Code, out PlayAnimationSO animation);
            return animation?.GetAudioByName(cardEffect.Localization.Code);
        }

        public AudioClip DefaultAudioClip() => _allAnimations["Default"].Audio[0];

    }
}
