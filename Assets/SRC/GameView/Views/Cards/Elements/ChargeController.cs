using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChargeController : MonoBehaviour, IStatable
    {
        private readonly List<SpriteRenderer> _charges = new();
        [SerializeField, Required, AssetsOnly] private Sprite _bullet;
        [SerializeField, Required, AssetsOnly] private Sprite _magic;
        [SerializeField, Required, AssetsOnly] private Sprite _supplie;
        [SerializeField, Required, AssetsOnly] private Sprite _secret;
        [SerializeField, Required, AssetsOnly] private SpriteRenderer _charge;
        [Inject] private readonly StatableManager _statableManager;

        public Stat Stat { get; private set; }
        public Transform StatTransform => _charges.First(charge => charge.transform.gameObject.activeSelf).transform;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IChargeable chargeable)
            {
                Stat = chargeable.Charge.Amount;
                _statableManager.Add(this);

                _charge.sprite = chargeable.Charge.ChargeType switch
                {
                    ChargeType.Bullet => _bullet,
                    ChargeType.MagicCharge => _magic,
                    ChargeType.Supplie => _supplie,
                    ChargeType.Secret => _secret,
                    _ => null
                };

                UpdateAnimation();

            }
            else gameObject.SetActive(false);
        }

        public Tween UpdateAnimation()
        {
            if (Stat.Value > _charges.Count)
            {
                for (int i = _charges.Count; i < Stat.Value; i++)
                {
                    _charges.Add(Instantiate(_charge, transform));
                }
            }
            else if (Stat.Value < _charges.Count)
            {
                for (int i = _charges.Count - 1; i >= Stat.Value; i--)
                {
                    Destroy(_charges[i].gameObject);
                    _charges.RemoveAt(i);
                }
            }

            return DOTween.Sequence();
        }
    }
}
