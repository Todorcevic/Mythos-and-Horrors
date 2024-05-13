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
            _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
            MustBeRevealedThisToken(ChallengeTokenType.Creature);

            CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
            Investigator investigator = _investigatorsProvider.First;
            Task<(ChallengeToken token, int tokenValue)> captureTokenTask = CaptureToken(investigator);
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(captureTokenTask.Result.tokenValue, Is.EqualTo(-1));
        }

        //[UnityTest]
        //public IEnumerator HardCreatureTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
        //    MustBeRevealedThisToken(ChallengeTokenType.Creature);

        //    Investigator investigator = _investigatorsProvider.First;

        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);

        //    Assert.That(_cardsProvider.GetCards<CardCreature>()
        //    .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Any(), Is.False);

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;

        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(challengeValue, Is.EqualTo(-3));
        //    Assert.That(_cardsProvider.GetCards<CardCreature>()
        //        .Where(creature => creature.IsInPlay && creature.HasThisTag(Tag.Monster)).Any(), Is.True);

        //}

        //[UnityTest]
        //public IEnumerator NormalCultistTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
        //    MustBeRevealedThisToken(ChallengeTokenType.Cultist);
        //    CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
        //    Investigator investigator = _investigatorsProvider.First;
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 3)).AsCoroutine();

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(monster));
        //    Assert.That(challengeValue, Is.EqualTo(-2));
        //    Assert.That(monster.Eldritch.Value, Is.EqualTo(4));
        //}

        //[UnityTest]
        //public IEnumerator HardCultistTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
        //    MustBeRevealedThisToken(ChallengeTokenType.Cultist);
        //    CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
        //    Investigator investigator = _investigatorsProvider.First;
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 3)).AsCoroutine();

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(investigator.NearestCreatures.First(), Is.EqualTo(monster));
        //    Assert.That(challengeValue, Is.EqualTo(-4));
        //    Assert.That(monster.Eldritch.Value, Is.EqualTo(5));
        //}

        //[UnityTest]
        //public IEnumerator HardCultistTokenTestWithoutAcolits()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
        //    MustBeRevealedThisToken(ChallengeTokenType.Cultist);
        //    Investigator investigator = _investigatorsProvider.First;
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;

        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();
        //    Assert.That(challengeValue, Is.EqualTo(-4));
        //}

        //[UnityTest]
        //public IEnumerator NormalDangerTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
        //    MustBeRevealedThisToken(ChallengeTokenType.Danger);
        //    Investigator investigator = _investigatorsProvider.First;
        //    CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, investigator.CurrentPlace.OwnZone)).AsCoroutine();

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(challengeValue, Is.EqualTo(-3));
        //    Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        //}

        //[UnityTest]
        //public IEnumerator HardDangerTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
        //    MustBeRevealedThisToken(ChallengeTokenType.Danger);
        //    Investigator investigator = _investigatorsProvider.First;
        //    CardCreature monster = (CardCreature)_preparationSceneCORE3.SceneCORE3.Hastur.First();
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(monster, investigator.CurrentPlace.OwnZone)).AsCoroutine();

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(challengeValue, Is.EqualTo(-5));
        //    Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        //    Assert.That(investigator.FearRecived, Is.EqualTo(1));
        //}

        //[UnityTest]
        //public IEnumerator HardDangerTokenTestNoMonster()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
        //    MustBeRevealedThisToken(ChallengeTokenType.Danger);
        //    Investigator investigator = _investigatorsProvider.First;
        //    yield return _preparationSceneCORE3.PlaceAllScene();
        //    yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);

        //    Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
        //    if (!DEBUG_MODE) yield return WaitToClick(investigator.CurrentPlace);
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed?.Sum(token => token.Value.Invoke()) == null) yield return null;
        //    int challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(challengeValue, Is.EqualTo(-5));
        //    Assert.That(investigator.DamageRecived, Is.EqualTo(0));
        //    Assert.That(investigator.FearRecived, Is.EqualTo(0));
        //}

        //[UnityTest]
        //public IEnumerator NormalAncientTokenTest()
        //{
        //    _chaptersProvider.SetCurrentDificulty(Dificulty.Normal);
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
        //    Assert.That(challengeValue, Is.EqualTo(-5));

        //    MustBeRevealedThisToken(ChallengeTokenType.Value_2);

        //    while (_gameActionsProvider.CurrentChallenge?.TokensRevealed.Count() < 2) yield return null;
        //    challengeValue = _gameActionsProvider.CurrentChallenge.TotalTokenRevealed;
        //    if (!DEBUG_MODE) yield return WaitToMainButtonClick();
        //    yield return taskGameAction.AsCoroutine();

        //    Assert.That(challengeValue, Is.EqualTo(-7));
        //}

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
