using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeBagComponent : MonoBehaviour
    {
        private readonly List<ChallengeTokenView> _allTokensDrop = new();
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [SerializeField, Required, AssetsOnly] private ChallengeTokenView _valueTokenPrefab;
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenView> _tokensPool;

        /*******************************************************************/
        public async Task DropToken(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = GetTokenView(realToken);
            tokenView.SetValue(realToken);
            tokenView.gameObject.SetActive(true);
            _allTokensDrop.Add(tokenView);
            await tokenView.PushUp();
        }

        public Tween RestoreAllTokens()
        {
            Sequence sequence = DOTween.Sequence();
            foreach (ChallengeTokenView tokenView in _allTokensDrop)
                sequence.Append(tokenView.Restore(_zoneViewsManager.CenterShowZone.transform, transform)
                    .OnComplete(() => _allTokensDrop.Remove(tokenView)));
            return sequence;
        }

        public Tween RestoreToken(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = _allTokensDrop.First(token => token.ChallengeToken == realToken);
            return tokenView.Restore(_zoneViewsManager.CenterShowZone.transform, transform)
                .OnComplete(() => _allTokensDrop.Remove(tokenView));
        }

        public Tween ShowCenter(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = _allTokensDrop.First(token => token.ChallengeToken == realToken);
            return tokenView.ShowCenter(_zoneViewsManager.CenterShowZone.transform);
        }

        public Tween ShakeToken(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = _allTokensDrop.First(token => token.ChallengeToken == realToken);
            return tokenView.ShakeToken();
        }

        private ChallengeTokenView GetTokenView(ChallengeToken tokens)
        {
            ChallengeTokenView tokenPrefab = _tokensPool.FirstOrDefault(tokenView => tokenView.Type == tokens.TokenType) ?? _valueTokenPrefab;
            return Instantiate(tokenPrefab, transform);
        }
    }
}
