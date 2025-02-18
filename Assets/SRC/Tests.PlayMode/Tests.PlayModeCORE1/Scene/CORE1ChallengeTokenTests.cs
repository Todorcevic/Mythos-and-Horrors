using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CORE1ChallengeTokenTests : TestCORE1Preparation
    {
        private Investigator investigator;
        private CardCreature ghoul;
        private Task<int> tokenValue;
        private Task revealToken;
        private Task taskGameAction;

        //protected override TestsType TestsType => TestsType.Debug;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            investigator = _investigatorsProvider.First;
            ghoul = SceneCORE1.GhoulSecuaz;
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
            taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
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
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, investigator.DangerZone).Execute().AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-1));
        }

        [UnityTest]
        public IEnumerator HardCreatureTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Creature);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, investigator.DangerZone).Execute().AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(_cardsProvider.GetCardsInPlay().OfType<CardCreature>().Count(), Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator NormalCultistTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Cultist);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Cellar).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, investigator.DangerZone).Execute().AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardCultistTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Cultist);
            revealToken.ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            Task<(int totalTokenAmount, int totalTokenValue)> totalTokensRevealed = CaptureTotalTokensRevelaed();

            //Task<ChallengePhaseGameAction> gdfgd = CaptureResolvingChallenge();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, investigator.DangerZone).Execute().AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(0));
            Assert.That(totalTokensRevealed.Result.totalTokenAmount, Is.EqualTo(2));
            Assert.That(totalTokensRevealed.Result.totalTokenValue, Is.EqualTo(-2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NormalDangerTokenTest()
        {
            SetScene(Dificulty.Normal, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, investigator.DangerZone).Execute().AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-2));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator HardDangerTokenTest()
        {
            SetScene(Dificulty.Hard, ChallengeTokenType.Danger);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ghoul, investigator.DangerZone).Execute().AsCoroutine();

            yield return ExecuteChallenge();

            Assert.That(tokenValue.Result, Is.EqualTo(-4));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }
    }
}