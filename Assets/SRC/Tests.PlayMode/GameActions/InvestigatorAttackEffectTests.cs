using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorAttackEffectTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatorAttackInDangerZoneTest()
        {
            _reactionableControl.SubscribeAtStart(RevealPlus1Token);
            CardCreature creature = _preparationScene.SceneCORE1.GhoulSecuaz;

            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        [UnityTest]
        public IEnumerator InvestigatorAttackInPlaceZoneTest()
        {
            _reactionableControl.SubscribeAtStart(RevealPlus1Token);
            CardCreature creature = _preparationScene.SceneCORE1.GhoulSecuaz;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.CurrentPlace.OwnZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(creature.Health.Value, Is.EqualTo(creature.Info.Health - 1));
        }

        private async Task RevealPlus1Token(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken minus1Token = _challengeTokensProvider.ChallengeTokensInBag
                .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Value1);
            revealChallengeTokenGameAction.SetChallengeToken(minus1Token);

            await Task.CompletedTask;
        }
    }
}
