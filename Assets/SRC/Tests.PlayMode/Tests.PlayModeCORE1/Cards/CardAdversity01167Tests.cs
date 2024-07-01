using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01167Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator FailChallengeAndReciveDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01167 cardAdversity = _cardsProvider.GetCard<Card01167>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Task drawTask = _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity));
            yield return ClickedMainButton();
            yield return drawTask.AsCoroutine();
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator FailChallengeAndDiscard()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01167 cardAdversity = _cardsProvider.GetCard<Card01167>();
            Card01506 supply = _cardsProvider.GetCard<Card01506>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();

            Task drawTask = _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity));
            yield return ClickedMainButton();
            yield return drawTask.AsCoroutine();
            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}