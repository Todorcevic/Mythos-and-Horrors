using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class ChallengeTokenTests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);
            yield return _preparationSceneCORE1.PlaceAllScene();
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, _investigatorsProvider.First.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(_investigatorsProvider.First)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenValue;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-1));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);
            yield return _preparationSceneCORE1.PlaceAllScene();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenValue;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-2));
            Assert.That(_cardsProvider.GetCardsInPlay().OfType<CardCreature>().Count(), Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            yield return _preparationSceneCORE1.PlaceAllScene();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, _preparationSceneCORE1.SceneCORE1.Cellar)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenValue;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-1));
            Assert.That(investigator.FearRecived, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            yield return _preparationSceneCORE1.PlaceAllScene();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.Sum(token => token.Value.Invoke(investigator));
            Assert.That(challengeValue, Is.EqualTo(0));
            MustBeRevealedThisToken(ChallengeTokenType.Danger);

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed.Count() < 2) yield return null;
            challengeValue = _gameActionsProvider.CurrentChallenge.TokensRevealed.Sum(token => token.Value.Invoke(investigator));

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-4));
            Assert.That(investigator.FearRecived, Is.EqualTo(4));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            yield return _preparationSceneCORE1.PlaceAllScene();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenValue;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-2));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            yield return _preparationSceneCORE1.PlaceAllScene();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationSceneCORE1.SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToClick(_investigatorsProvider.First.InvestigatorCard);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenValue;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-4));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
            Assert.That(investigator.FearRecived, Is.EqualTo(2));
        }
    }
}
