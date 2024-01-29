using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokenController : MonoBehaviour, IStatableView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _allTokens;

        public int Amount => _allTokens.Count(tokenView => tokenView.IsActive);
        public TokenView TokenOn => _allTokens.Last(tokenView => tokenView.IsActive);
        public TokenView TokenOff => _allTokens.Find(tokenView => !tokenView.IsActive) ?? CreateNewToken();
        public Stat Stat { get; private set; }
        public Transform StatTransform => TokenOn.transform;

        /*******************************************************************/
        private TokenView CreateNewToken()
        {
            TokenView newToken = _allTokens.Last().Clone();
            _allTokens.Add(newToken);
            return newToken;
        }

        public Tween UpdateValue()
        {
            return TokenOn.SetAmount(Stat.Value);
        }

        public void SetStat(Stat stat)
        {
            Stat = stat;
        }
    }
}
