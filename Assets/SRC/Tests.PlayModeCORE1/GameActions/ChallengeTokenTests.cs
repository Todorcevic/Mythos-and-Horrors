using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChallengeTokenTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            RevealToken(ChallengeTokenType.Creature);
            yield return _preparationScene.PlaceAllSceneCORE1();
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, _investigatorsProvider.First.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value.Invoke();

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-1));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            RevealToken(ChallengeTokenType.Creature);
            yield return _preparationScene.PlaceAllSceneCORE1();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value.Invoke();

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-2));
            Assert.That(_cardsProvider.GetCardsInPlay().OfType<CardCreature>().Count(), Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            RevealToken(ChallengeTokenType.Cultist);
            yield return _preparationScene.PlaceAllSceneCORE1();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, _preparationScene.SceneCORE1.Cellar)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value.Invoke();

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            RevealToken(ChallengeTokenType.Cultist);
            yield return _preparationScene.PlaceAllSceneCORE1();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.Sum(token => token.Value.Invoke());
            Assert.That(challengeValue, Is.EqualTo(0));
            RevealToken(ChallengeTokenType.Danger);

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed.Count() < 2) yield return null;
            challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.Sum(token => token.Value.Invoke());

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-4));
            Assert.That(investigator.FearRecived, Is.EqualTo(3));
            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            RevealToken(ChallengeTokenType.Danger);
            yield return _preparationScene.PlaceAllSceneCORE1();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value.Invoke();

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-2));
            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            RevealToken(ChallengeTokenType.Danger);
            yield return _preparationScene.PlaceAllSceneCORE1();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.First().Value.Invoke();

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-4));
            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }
    }
}
