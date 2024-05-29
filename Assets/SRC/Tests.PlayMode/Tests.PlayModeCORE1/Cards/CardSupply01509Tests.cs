using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01509Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator MoveToDangeZone()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardSupply Necronomicon = _cardsProvider.GetCard<Card01509>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Necronomicon, investigator.DeckZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new DrawAidGameAction(investigator)).AsCoroutine();

            Assert.That(Necronomicon.CurrentZone, Is.EqualTo(investigator.DangerZone));
        }

        [UnityTest]
        public IEnumerator RevealFailToken()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardSupply Necronomicon = _cardsProvider.GetCard<Card01509>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            Task<ChallengePhaseGameAction> challengePhase = CaptureResolvingChallenge();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Necronomicon, investigator.DangerZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(challengePhase.Result.IsAutoFail, Is.True);
            Assert.That(investigator.Hints.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator TakeFear()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01509 Necronomicon = _cardsProvider.GetCard<Card01509>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Necronomicon, investigator.DangerZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(Necronomicon);
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
            Assert.That(Necronomicon.Fear.Value, Is.EqualTo(2));
            yield return ClickedIn(Necronomicon);
            yield return ClickedIn(Necronomicon);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(Necronomicon.CurrentZone, Is.EqualTo(Necronomicon.Owner.DiscardZone));
            Assert.That(investigator.FearRecived, Is.EqualTo(3));
        }
    }
}
