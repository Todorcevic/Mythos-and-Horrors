using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardAdversity01158Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator FailChallenge()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01158 cardAdversity = _cardsProvider.GetCard<Card01158>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            Task drawTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.DiscardZone.Cards.Count(), Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator FailChallengeAnDie()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01158 cardAdversity = _cardsProvider.GetCard<Card01158>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(_investigatorsProvider.Second);

            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>()
                .SetWith(investigator, cardAdversity, amountFear: 4, isInevitable: true).Execute().AsCoroutine();

            Task drawTask = _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Execute();
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.IsInPlay.IsTrue, Is.False);
        }
    }
}
