using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class BadgeController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private Sprite _brave;
        [SerializeField, Required, AssetsOnly] private Sprite _cunning;
        [SerializeField, Required, AssetsOnly] private Sprite _esoteric;
        [SerializeField, Required, AssetsOnly] private Sprite _scholarly;
        [SerializeField, Required, AssetsOnly] private Sprite _versatile;
        [SerializeField, Required, AssetsOnly] private Sprite _neutral;
        [SerializeField, Required, AssetsOnly] private Sprite _myth;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;

        /*******************************************************************/
        public void SetBadge(Faction faction)
        {
            _badge.sprite = faction switch
            {
                Faction.Brave => _brave,
                Faction.Cunning => _cunning,
                Faction.Esoteric => _esoteric,
                Faction.Scholarly => _scholarly,
                Faction.Versatile => _versatile,
                Faction.Neutral => _neutral,
                Faction.Myth => _myth,
                Faction.None => _neutral,
                _ => _neutral
            };
        }
    }
}
