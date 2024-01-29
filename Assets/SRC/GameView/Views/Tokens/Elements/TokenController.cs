using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class TokenController : MonoBehaviour, IStatableView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _allTokens;
        [Inject] private readonly ZoneViewsManager _zonesManager;

        public int Amount => _allTokens.Count(tokenView => tokenView.IsActive);
        public TokenView TokenOn => _allTokens.Last(tokenView => tokenView.IsActive);
        public TokenView TokenOff => _allTokens.Find(tokenView => !tokenView.IsActive) ?? CreateNewToken();
        public Stat Stat { get; private set; }
        public Transform StatTransform => TokenOn.transform;
        private Transform CenterShow => _zonesManager.CenterShowZone.transform;

        /*******************************************************************/
        public void Init(Stat stat)
        {
            Stat = stat;
        }

        /*******************************************************************/
        public Tween Pay(int amount, Transform thisPosition)
        {
            Sequence paySequence = DOTween.Sequence();
            GetActiveTokens(amount).ForEach(token => paySequence.Join(token.MoveTo(thisPosition, CenterShow)));
            return paySequence;
        }

        public Tween Gain(int amount, Transform thisPosition)
        {
            Sequence gainSequence = DOTween.Sequence();
            GetDeactiveTokens(amount).ForEach(token => gainSequence.Join(token.MoveFrom(thisPosition, CenterShow)));
            return gainSequence;
        }

        public Tween UpdateValue() => TokenOn.SetAmount(Stat.Value);

        private TokenView CreateNewToken()
        {
            TokenView newToken = _allTokens.Last().Clone();
            _allTokens.Add(newToken);
            return newToken;
        }

        private List<TokenView> GetDeactiveTokens(int amount)
        {
            int deactiveTokensAmount = _allTokens.Count(tokenView => !tokenView.IsActive);
            if (deactiveTokensAmount < amount)
                _allTokens.AddRange(Enumerable.Repeat(CreateNewToken(), amount - deactiveTokensAmount));

            return _allTokens.Where(tokenView => !tokenView.IsActive).Take(amount).ToList();
        }

        private List<TokenView> GetActiveTokens(int amount) =>
            _allTokens.Where(tokenView => tokenView.IsActive).Take(amount).ToList();

    }
}
