using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AvatarViewsManager
    {
        [Inject] private readonly List<AvatarView> _allAvatars;

        public List<AvatarView> AllAvatars => _allAvatars.OrderBy(avatarView => avatarView.name).Where(av => !av.IsVoid).ToList();

        /*******************************************************************/
        public AvatarView GetVoid() => _allAvatars.OrderBy(avatarView => avatarView.name).First(avatarView => avatarView.IsVoid);

        public AvatarView Get(Investigator investigator) => AllAvatars.Find(avatarView => avatarView.Investigator == investigator);

        public List<AvatarView> AvatarsPlayabled(List<Card> cardsPlayabled) =>
            AllAvatars.FindAll(avatar => avatar.Investigator.Cards.Any(card => cardsPlayabled.Contains(card)));

        public AvatarView GetByCode(string code) => AllAvatars.Find(avatarView => avatarView.Investigator.Code == code);
    }
}
