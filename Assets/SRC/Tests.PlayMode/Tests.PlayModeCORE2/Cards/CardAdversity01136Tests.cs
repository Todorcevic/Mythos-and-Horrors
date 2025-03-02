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
        public IEnumerator NoKeys()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01136 adversityCard = _cardsProvider.GetCard<Card01136>();
            Card01135 adversityCard2 = _cardsProvider.GetCard<Card01135>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard2, SceneCORE2.DangerDeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, SceneCORE2.DangerDeckZone, isFaceDown: true).Execute().AsCoroutine();
            Task task = _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(adversityCard2);
            yield return task.AsCoroutine();
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
            int keysPlaceExpected = investigator.CurrentPlace.Keys.Value - 1;
            yield return _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 2).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(adversityCard, SceneCORE2.DangerDeckZone, isFaceDown: true).Execute().AsCoroutine();
            Assert.That(investigator.Keys.Value, Is.EqualTo(2));

            Task drawTask = _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute();
            yield return ClickedMainButton();
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
            Assert.That(investigator.CurrentPlace.Keys.Value, Is.EqualTo(keysPlaceExpected));
            Assert.That(adversityCard.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
        }
    }
}
