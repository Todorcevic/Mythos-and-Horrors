using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChallengeTokenSceneCORE2Tests : TestCORE2PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);
            CardCreature acolit = _preparationSceneCORE2.SceneCORE2.Acolits.First();
            CardCreature acolit2 = _preparationSceneCORE2.SceneCORE2.Acolits.Skip(1).First();
            Investigator investigator = _investigatorsProvider.First;

            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, _preparationSceneCORE2.SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();


            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-4));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);
            CardCreature acolit = _preparationSceneCORE2.SceneCORE2.Acolits.First();
            CardCreature acolit2 = _preparationSceneCORE2.SceneCORE2.Acolits.Skip(1).First();
            Investigator investigator = _investigatorsProvider.First;

            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, _preparationSceneCORE2.SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(_preparationSceneCORE2.SceneCORE2.CurrentPlot.Eldritch, 2)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;

            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-8));
        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            CardCreature acolit = _preparationSceneCORE2.SceneCORE2.Acolits.First();
            CardCreature acolit2 = _preparationSceneCORE2.SceneCORE2.Acolits.Skip(1).First();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, _preparationSceneCORE2.SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(acolit2));
            Assert.That(challengeValue, Is.EqualTo(-2));
            Assert.That(acolit2.Eldritch.Value, Is.EqualTo(3)); //+1 for spawn +1 for increment +1 for challenge
            Assert.That(acolit.Eldritch.Value, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            CardCreature acolit = _preparationSceneCORE2.SceneCORE2.Acolits.First();
            CardCreature acolit2 = _preparationSceneCORE2.SceneCORE2.Acolits.Skip(1).First();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit, _preparationSceneCORE2.SceneCORE2.North.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit.Eldritch, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(acolit2, _preparationSceneCORE2.SceneCORE2.West.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(acolit2.Eldritch, 1)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-2));
            Assert.That(acolit2.Eldritch.Value, Is.EqualTo(3)); //+1 for spawn +1 for increment +1 for challenge
            Assert.That(acolit.Eldritch.Value, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTestWithoutAcolits()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
            Assert.That(challengeValue, Is.EqualTo(-2));

            MustBeRevealedThisToken(ChallengeTokenType.Value_2);

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed.Count() < 2) yield return null;
            challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();
            Assert.That(challengeValue, Is.EqualTo(-4));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, _preparationSceneCORE2.SceneCORE2.North.Hints, 2)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-3));
            Assert.That(_preparationSceneCORE2.SceneCORE2.North.Hints.Value, Is.EqualTo(6));
            Assert.That(_preparationSceneCORE2.SceneCORE2.Fluvial.Hints.Value, Is.EqualTo(5));
            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE2.PlaceAllScene();
            yield return _preparationSceneCORE2.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, _preparationSceneCORE2.SceneCORE2.North.Hints, 2)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke(investigator)) == null) yield return null;
            int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            yield return taskGameAction.AsCoroutine();

            Assert.That(challengeValue, Is.EqualTo(-4));
            Assert.That(_preparationSceneCORE2.SceneCORE2.North.Hints.Value, Is.EqualTo(6));
            Assert.That(_preparationSceneCORE2.SceneCORE2.Fluvial.Hints.Value, Is.EqualTo(6));
            Assert.That(investigator.Hints.Value, Is.EqualTo(0));
        }
    }
}
