using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class CORE1ChallengeTokenTests : TestCORE1PlayModeBase
    {
        private Investigator investigator;
        private CardCreature ghoul;
        private Task<int> tokenValue;
        private Task revealToken;
        private Task taskGameAction;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            investigator = _investigatorsProvider.First;
            ghoul = _preparationSceneCORE1.SceneCORE1.GhoulSecuaz;
            tokenValue = CaptureTokenValue(investigator);
            yield return _preparationSceneCORE1.PlaceAllScene().AsCoroutine();
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator).AsCoroutine();
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
            FakeInteractablePresenter.ClickedIn(investigator.CurrentPlace);
            FakeInteractablePresenter.ClickedIn(investigator.InvestigatorCard);
            FakeInteractablePresenter.ClickedMainButton();
            FakeInteractablePresenter.ClickedMainButton();
            yield return taskGameAction.AsCoroutine();
        }

        /*******************************************************************/
        [UnityTest]
        public IEnumerator NormalCreatureTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Creature);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, investigator.DangerZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-1));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Creature);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, investigator.DangerZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(_cardsProvider.GetCardsInPlay().OfType<CardCreature>().Count(), Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Cultist);
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, _preparationSceneCORE1.SceneCORE1.Cellar)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, investigator.DangerZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-1));
            Assert.That(investigator.FearRecived, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Cultist);
            revealToken.ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, investigator.DangerZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(0));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-2));
            Assert.That(investigator.FearRecived, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, investigator.DangerZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(ghoul, investigator.DangerZone)).AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-4));
            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
            Assert.That(investigator.FearRecived, Is.EqualTo(2));
        }
    }
}
