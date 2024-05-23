using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    public class ChallengeTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PushTokenTest()
        {
            ChallengeToken challengeToken = new(ChallengeTokenType.Ancient, (_) => -2);
            do
            {
                if (DEBUG_MODE) yield return PressAnyKey();

                yield return _challengeBagComponent.DropToken(challengeToken, _investigatorsProvider.First).WaitForCompletion();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Unique().ChallengeToken,
                Is.EqualTo(challengeToken));

            yield return _challengeBagComponent.RestoreToken(challengeToken).WaitForCompletion();

            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Count, Is.EqualTo(0));
        }
    }
}
