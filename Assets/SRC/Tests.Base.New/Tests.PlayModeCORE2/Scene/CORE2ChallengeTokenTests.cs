﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CORE2ChallengeTokenTests : TestCORE2Preparation
    {
        private Investigator investigator;
        private CardCreature acolit;
        private CardCreature acolit2;
        private Task<int> tokenValue;
        private Task revealToken;
        private Task taskGameAction;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            investigator = _investigatorsProvider.First;
            acolit = SceneCORE2.Acolits.ElementAt(0);
            acolit2 = SceneCORE2.Acolits.ElementAt(1);
            tokenValue = CaptureTokenValue(investigator);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
        }

        /*******************************************************************/
        private void SetScene(Dificulty dificulty, ChallengeTokenType challengeTokenType)
        {
            _chaptersProvider.SetCurrentDificulty(dificulty);
            revealToken = MustBeRevealedThisToken(challengeTokenType);
        }

        private IEnumerator ExecuteChallenge()
        {
            taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Creature);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-4));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Creature);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(SceneCORE2.CurrentPlot.Eldritch, 2)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-8));
        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Cultist);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(acolit2));
            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(acolit2.Eldritch.Value, Is.EqualTo(3)); //+1 for spawn +1 for increment +1 for challenge
            Assert.That(acolit.Eldritch.Value, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Cultist);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(acolit2.Eldritch.Value, Is.EqualTo(3)); //+1 for spawn +1 for increment +1 for challenge
            Assert.That(acolit.Eldritch.Value, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTestWithoutAcolits()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Cultist);
            revealToken.ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-4));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, SceneCORE2.North.Hints, 2)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-3));
            Assert.That(SceneCORE2.North.Hints.Value, Is.EqualTo(6));
            Assert.That(SceneCORE2.Fluvial.Hints.Value, Is.EqualTo(5));
            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, SceneCORE2.North.Hints, 2)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-4));
            Assert.That(SceneCORE2.North.Hints.Value, Is.EqualTo(6));
            Assert.That(SceneCORE2.Fluvial.Hints.Value, Is.EqualTo(6));
            Assert.That(investigator.Hints.Value, Is.EqualTo(0));
        }
    }
}