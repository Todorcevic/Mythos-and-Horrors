
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardAdversity01135Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator PayClue()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01135 adversityCard = _cardsProvider.GetCard<Card01135>();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 2)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversityCard, SceneCORE2.DangerDeckZone, isFaceDown: true)).AsCoroutine();
            Assert.That(investigator.Hints.Value, Is.EqualTo(2));

            Task<DrawDangerGameAction> drawTask = _gameActionsProvider.Create(new DrawDangerGameAction(investigator));
            yield return ClickedClone(adversityCard, 0);
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(adversityCard.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
        }

        [UnityTest]
        public IEnumerator TakeDamage()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01135 adversityCard = _cardsProvider.GetCard<Card01135>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(adversityCard, SceneCORE2.DangerDeckZone, isFaceDown: true)).AsCoroutine();

            Task<DrawDangerGameAction> drawTask = _gameActionsProvider.Create(new DrawDangerGameAction(investigator));
            yield return ClickedClone(adversityCard, 1);
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(2));
            Assert.That(adversityCard.CurrentZone, Is.EqualTo(SceneCORE2.DangerDiscardZone));
        }
    }
}
