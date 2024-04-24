using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class TokenController : MonoBehaviour, IStatable
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _allTokens;
        [Inject(Id = ZenjectBinding.BindId.CenterShowToken)] public Transform CenterShow { get; }
        [Inject] private readonly StatableManager _statableManager;

        public int Amount => _allTokens.Count(tokenView => tokenView.IsActive);
        public TokenView TokenOn => _allTokens.Last(tokenView => tokenView.IsActive);
        public TokenView TokenOff => _allTokens.Find(tokenView => !tokenView.IsActive) ?? CreateNewToken();
        public Stat Stat { get; private set; }
        public Transform StatTransform => TokenOn.transform;

        /*******************************************************************/
        public void Init(Stat stat)
        {
            Stat = stat;
            _statableManager.Add(this);
        }

        Tween IStatable.UpdateValue()
        {
            Sequence updateSequence = DOTween.Sequence();
            _allTokens.ForEach(token => updateSequence.Join(token.SetAmount(_allTokens.IndexOf(token) + 1)));
            return updateSequence;
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
            {
                for (int i = 0; i < amount - deactiveTokensAmount; i++)
                {
                    CreateNewToken();
                }
            }

            List<TokenView> deactiveTokens = new();
            for (int i = 0; i < amount; i++)
            {
                TokenView token = _allTokens.First(tokenView => !tokenView.IsActive && !deactiveTokens.Contains(tokenView));
                deactiveTokens.Add(token);
            }
            return deactiveTokens;
        }

        private List<TokenView> GetActiveTokens(int amount)
        {
            List<TokenView> activeTokens = new();
            for (int i = 0; i < amount; i++)
            {
                TokenView token = _allTokens.Last(tokenView => tokenView.IsActive && !activeTokens.Contains(tokenView));
                activeTokens.Add(token);
            }
            return activeTokens;
        }
    }
}
