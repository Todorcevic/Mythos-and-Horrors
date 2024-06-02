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

            Task<DrawGameAction> drawTask = _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity));
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return drawTask.AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
            Assert.That(investigator.FearRecived, Is.EqualTo(1));
            Assert.That(investigator.DiscardZone.Cards.Count(), Is.EqualTo(1));
        }
    }
}
