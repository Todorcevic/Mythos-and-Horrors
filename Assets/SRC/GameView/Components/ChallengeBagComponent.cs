using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeBagComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<ChallengeTokenView> _tokens;

        /*******************************************************************/

        public void CreateTokens(IEnumerable<ChallengeTokenType> tokens)
        {


        }

        public void DropToken(ChallengeTokenView token)
        {
            token.gameObject.SetActive(true);
            token.PushUp();
        }
    }
}
