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
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [SerializeField, Required, AssetsOnly] private ChallengeTokenView _valueTokenPrefab;
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenView> _tokensPool;

        /*******************************************************************/
        public async Task DropToken(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = GetTokenView(realToken);
            tokenView.SetValue(realToken.Value());
            tokenView.gameObject.SetActive(true);
            await tokenView.PushUp();
            await tokenView.Restore(_zoneViewsManager.CenterShowZone.transform, transform).AsyncWaitForCompletion();
        }

        private ChallengeTokenView GetTokenView(ChallengeToken tokens)
        {
            ChallengeTokenView tokenPrefab = _tokensPool.FirstOrDefault(tokenView => tokenView.Type == tokens.TokenType) ?? _valueTokenPrefab;
            return Instantiate(tokenPrefab, transform);
        }
    }
}
