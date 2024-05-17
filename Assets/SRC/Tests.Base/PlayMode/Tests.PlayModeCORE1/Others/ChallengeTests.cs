using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChallengeTests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PushTokenTest()
        {
            ChallengeToken challengeToken = new(ChallengeTokenType.Ancient, (_) => -2);
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);

            do
            {
                if (DEBUG_MODE) yield return PressAnyKey();

                yield return _challengeBagComponent.DropToken(challengeToken, _investigatorsProvider.First).AsCoroutine();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Unique().ChallengeToken,
                Is.EqualTo(challengeToken));

            yield return _challengeBagComponent.RestoreToken(challengeToken).WaitForCompletion();

            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Count, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator ChallengeWithCommitsTests()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return _preparationSceneCORE1.StartingScene();
            Card toPlay = _cardsProvider.GetCard<Card01538>();
            Card toPlay2 = _cardsProvider.GetCard<Card01522>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(0));
            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }
    }
}
