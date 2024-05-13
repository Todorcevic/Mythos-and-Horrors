using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CORE3ChallengeTokenTests : TestCORE3PlayModeBase
    {
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
            Investigator investigator = _investigatorsProvider.First;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-1));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);

            Assert.That(_cardsProvider.GetCards<CardCreature>()
            .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Any(), Is.False);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-3));
            Assert.That(_cardsProvider.GetCards<CardCreature>()
                .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Any(), Is.True);

        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
            Investigator investigator = _investigatorsProvider.First;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            Task<int> tokenValue = CaptureTokenValue(investigator);

            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 3)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(monster));
            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(monster.Eldritch.Value, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
            Investigator investigator = _investigatorsProvider.First;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            Task<int> tokenValue = CaptureTokenValue(investigator);

            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 3)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(monster));
            Assert.That(tokenValue.Result, Is.EqualTo(-4));
            Assert.That(monster.Eldritch.Value, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTestWithoutAcolits()
        {
            Investigator investigator = _investigatorsProvider.First;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Cultist);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-4));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, investigator.CurrentPlace.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-3));
            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, investigator.CurrentPlace.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-5));
            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTestNoMonster()
        {
            Investigator investigator = _investigatorsProvider.First;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            MustBeRevealedThisToken(ChallengeTokenType.Danger);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-5));
            Assert.That(investigator.DamageRecived, Is.EqualTo(0));
            Assert.That(investigator.FearRecived, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator NormalAncientTokenTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature ancient = _preparationSceneCORE3.SceneCORE3.Urmodoth;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Ancient);
            Task<int> tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ancient, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);

            FakeInteractablePresenter.ClickedMainButton();

            MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            Task<int> tokenValue2 = CaptureTokenValue(investigator);

            Assert.That(tokenValue.Result, Is.EqualTo(-5));

            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue2.Result, Is.EqualTo(-7));
        }

        //[UnityTest]
        //public IEnumerator HardAncientTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
        //    MustBeRevealedThisToken(ChallengeTokenType.Ancient);
        //    Investigator investigator = _investigatorsProvider.First;
        //    CardCreature ancient = _preparationSceneCORE3.SceneCORE3.Urmodoth;
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(ancient, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    Assert.That(challengeValue, Is.EqualTo(-7));

        //    MustBeRevealedThisToken(ChallengeTokenType.Value_2);

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed.Count() < 2) yield return null;
        //    challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(challengeValue, Is.EqualTo(-9));
        //}
    }
}
