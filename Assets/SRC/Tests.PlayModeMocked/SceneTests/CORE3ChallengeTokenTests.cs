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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Creature);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Creature);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Cultist);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Cultist);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Cultist);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Danger);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Danger);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Danger);
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
            _ = MustBeRevealedThisToken(ChallengeTokenType.Ancient)
                .ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<int> tokenValue = CaptureTokenValue(investigator);
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ancient, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-5));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-7));
        }

        [UnityTest]
        public IEnumerator HardAncientTokenTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature ancient = _preparationSceneCORE3.SceneCORE3.Urmodoth;
            _chaptersProvider.SetCurrentDificulty(Dificulty.Hard);
            _ = MustBeRevealedThisToken(ChallengeTokenType.Ancient)
                .ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<int> tokenValue = CaptureTokenValue(investigator);
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();
            yield return _preparationSceneCORE3.PlaceAllScene();
            yield return _preparationSceneCORE3.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ancient, _preparationSceneCORE3.SceneCORE3.Forest2.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(-7));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-9));
        }
    }
}
