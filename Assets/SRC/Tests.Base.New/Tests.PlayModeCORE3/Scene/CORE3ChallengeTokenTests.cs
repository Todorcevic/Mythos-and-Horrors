﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CORE3ChallengeTokenTests : TestCORE3Preparation
    {
        private Investigator investigator;
        private CardCreature monster;
        private CardCreature ancient;
        private Task<int> tokenValue;
        private Task revealToken;
        private Task taskGameAction;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            investigator = _investigatorsProvider.First;
            monster = (CardCreature)SceneCORE3.Hastur.First();
            ancient = SceneCORE3.Urmodoth;
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

        private IEnumerator ExecuteChallengeWithOpportunityAttack()
        {
            taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Creature);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-1));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Creature);
            Assert.That(_cardsProvider.GetCards<CardCreature>()
            .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Any(), Is.False);

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-3));
            Assert.That(_cardsProvider.GetCards<CardCreature>()
                .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Any(), Is.True);

        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Cultist);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, SceneCORE3.Forest2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 3)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(monster));
            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(monster.Eldritch.Value, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Cultist);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, SceneCORE3.Forest2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 3)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(monster));
            Assert.That(tokenValue.Result, Is.EqualTo(-4));
            Assert.That(monster.Eldritch.Value, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTestWithoutAcolits()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Cultist);

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-4));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, investigator.CurrentPlace.OwnZone)).AsCoroutine();

            yield return ExecuteChallengeWithOpportunityAttack();

            Assert.That(tokenValue.Result, Is.EqualTo(-3));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, investigator.CurrentPlace.OwnZone)).AsCoroutine();

            yield return ExecuteChallengeWithOpportunityAttack();

            Assert.That(tokenValue.Result, Is.EqualTo(-5));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
            Assert.That(investigator.FearRecived, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTestNoMonster()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Danger);

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-5));
            Assert.That(investigator.DamageRecived, Is.EqualTo(0));
            Assert.That(investigator.FearRecived, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator NormalAncientTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Ancient);
            revealToken.ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ancient, SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-5));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-7));
        }

        [UnityTest]
        public IEnumerator HardAncientTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Ancient);
            revealToken.ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ancient, SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-7));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-9));
        }
    }
}