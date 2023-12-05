using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AvatarViewsManager
    {
        [Inject] private readonly List<AvatarView> _allAvatars;

        /*******************************************************************/
        public AvatarView Get(Adventurer adventurer) => _allAvatars.First(avatarView => avatarView.Adventurer == adventurer);

        public List<AvatarView> AllAvatars => _allAvatars.Where(av => av.IsVoid).ToList();

        public void Init(Adventurer newAdvewnture) => _allAvatars.First(avatarView => avatarView.IsVoid).Init(newAdvewnture);

    }
}
