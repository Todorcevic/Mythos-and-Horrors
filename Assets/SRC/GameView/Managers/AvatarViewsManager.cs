using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AvatarViewsManager
    {
        [Inject] private readonly List<AvatarView> _allAvatars;

        public List<AvatarView> AllAvatars => _allAvatars.Where(av => !av.IsVoid).ToList();

        /*******************************************************************/
        public AvatarView GetVoid() => _allAvatars.First(avatarView => avatarView.IsVoid);

        public AvatarView Get(Adventurer adventurer) => _allAvatars.First(avatarView => avatarView.Adventurer == adventurer);
    }
}
