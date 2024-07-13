using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class ChargeController : MonoBehaviour, IStatable
    {
        [SerializeField, Required, AssetsOnly] private Sprite _bullet;
        [SerializeField, Required, AssetsOnly] private Sprite _magic;
        [SerializeField, Required, AssetsOnly] private Sprite _supplie;
        [SerializeField, Required, AssetsOnly] private Sprite _secret;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SpriteRenderer> _charges;

        public Stat Stat { get; private set; }
        public Transform StatTransform => _charges.First(charge => charge.transform.gameObject.activeSelf).transform;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is not IChargeable chargeable) return;
            Stat = chargeable.Charge.Amount;

            Sprite sprite = chargeable.Charge.ChargeType switch
            {
                ChargeType.Bullet => _bullet,
                ChargeType.MagicCharge => _magic,
                ChargeType.Supplie => _supplie,
                ChargeType.Secret => _secret,
                _ => null
            };

            _charges.ForEach(charge => charge.sprite = sprite);
            UpdateAnimation();
        }

        public Tween UpdateAnimation()
        {
            _charges.Take(Stat.Value).ForEach(charge => charge.gameObject.SetActive(true));
            _charges.Skip(Stat.Value).ForEach(charge => charge.gameObject.SetActive(false));
            return DOTween.Sequence();
        }
    }
}
