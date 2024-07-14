using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class BadgeController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private Sprite _brave;
        [SerializeField, Required, AssetsOnly] private Sprite _cunning;
        [SerializeField, Required, AssetsOnly] private Sprite _esoteric;
        [SerializeField, Required, AssetsOnly] private Sprite _scholarly;
        [SerializeField, Required, AssetsOnly] private Sprite _versatile;
        [SerializeField, Required, AssetsOnly] private Sprite _myth;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;

        /*******************************************************************/
        public void Init(Card card)
        {
            SetBadge(card.Info.Faction);
        }

        private void SetBadge(Faction faction)
        {
            _badge.sprite = faction switch
            {
                Faction.Brave => _brave,
                Faction.Cunning => _cunning,
                Faction.Esoteric => _esoteric,
                Faction.Scholarly => _scholarly,
                Faction.Versatile => _versatile,
                Faction.Myth => _myth,
                _ => null
            };

            if (_badge.sprite == null) Destroy(gameObject);
        }
    }
}
