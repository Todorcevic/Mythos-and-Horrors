using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardAdversity01136Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator NoHints()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01136 adversityCard = _cardsProvider.GetCard<Card01136>();
            Card01135 adversityCard2 = _cardsProvider.GetCard<Card01135>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard2, SceneCORE2.DangerDeckZone, isFaceDown: true).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, SceneCORE2.DangerDeckZone, isFaceDown: true).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Start().AsCoroutine();

            Assert.That(adversityCard2.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator FailChallenge()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01136 adversityCard = _cardsProvider.GetCard<Card01136>();
            int hintsPlaceExpected = investigator.CurrentPlace.Hints.Value - 1;
            yield return _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 2).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, SceneCORE2.DangerDeckZone, isFaceDown: true).Start().AsCoroutine();
            Assert.That(investigator.Hints.Value, Is.EqualTo(2));

            Task drawTask = _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Start();
            yield return ClickedMainButton();
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(investigator.CurrentPlace.Hints.Value, Is.EqualTo(hintsPlaceExpected));
            Assert.That(adversityCard.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
        }
    }
}
