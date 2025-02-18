using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeBagComponent : MonoBehaviour
    {
        private readonly List<ChallengeToken3DView> _allTokensDrop = new();
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [SerializeField, Required, SceneObjectsOnly] private ChallengeTokensManager _challengeTokensManager;

        /*******************************************************************/
        public Tween DropToken(ChallengeToken challengeToken)
        {
            ChallengeToken3DView challengeTokenView = _challengeTokensManager.GetTokenView(challengeToken, transform);
            challengeTokenView.gameObject.SetActive(true);
            _allTokensDrop.Add(challengeTokenView);
            return challengeTokenView.PushUp();
        }

        public Tween RestoreToken(ChallengeToken realToken)
        {
            ChallengeToken3DView tokenView = _allTokensDrop.First(token => token.ChallengeToken == realToken);
            return tokenView.RestoreAndDestroy(_zoneViewsManager.CenterShowZone.transform, transform)
                .OnComplete(() => _allTokensDrop.Remove(tokenView));
        }

        public Tween ShakeToken(ChallengeToken realToken)
        {
            ChallengeToken3DView tokenView = _allTokensDrop.First(token => token.ChallengeToken == realToken);
            return tokenView.ShakeToken(_zoneViewsManager.CenterShowZone.transform);
        }
    }
}
