using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class AvatarViewsManager
    {
        [Inject] private readonly List<AvatarView> _allAvatars;

        public List<AvatarView> AllAvatars => _allAvatars.OrderBy(avatarView => avatarView.name).Where(av => !av.IsVoid).ToList();

        /*******************************************************************/
        public AvatarView GetVoid() => _allAvatars.OrderBy(avatarView => avatarView.name).First(avatarView => avatarView.IsVoid);

        public AvatarView Get(Investigator investigator) => AllAvatars.Find(avatarView => avatarView.Investigator == investigator);

        public List<AvatarView> AvatarsPlayabled(List<IPlayable> specificsCardViews)
        {
            IEnumerable<Investigator> investigatorsToActivate = specificsCardViews
                .SelectMany(playable => playable.EffectsSelected)
                .Select(effect => effect.Investigator);
            return AllAvatars.FindAll(avatar => investigatorsToActivate.Contains(avatar.Investigator));
        }

        public AvatarView GetByCode(string code) => AllAvatars.Find(avatarView => avatarView.Investigator.Code == code);
    }
}
