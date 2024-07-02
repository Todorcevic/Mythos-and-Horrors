
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01535Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SusceeChallenge()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Investigator investigator2 = _investigatorsProvider.First;
            Card01535 cardSupply = _cardsProvider.GetCard<Card01535>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator2, cardSupply, amountDamage: 2).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.DamageRecived.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator FailChallenge()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Investigator investigator2 = _investigatorsProvider.First;
            Card01535 cardSupply = _cardsProvider.GetCard<Card01535>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator2, cardSupply, amountDamage: 2).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator2.DamageRecived.Value, Is.EqualTo(3));
        }
    }
}
