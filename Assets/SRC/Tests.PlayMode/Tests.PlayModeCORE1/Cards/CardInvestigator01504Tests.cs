using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardInvestigator01504Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator StarChallengeTokenRevealed()
        {
            _ = MustBeRevealedThisToken(ChallengeTokenType.Star);
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            Task<int> tokenValue = CaptureTokenValue(investigatorToTest);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigatorToTest);
            yield return _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigatorToTest.FearRecived, 3).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigatorToTest));
            yield return ClickedIn(investigatorToTest.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(tokenValue.Result, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator HarmCreatureWhenTakeFear()
        {
            CardInvestigator cardInvestigator = _cardsProvider.GetCard<Card01504>();
            Investigator investigatorToTest = cardInvestigator.Owner;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            yield return StartingScene();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigatorToTest.CurrentPlace.OwnZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigatorToTest, creature, amountFear: 3, isDirect: true).Start();
            yield return ClickedIn(cardInvestigator);
            yield return ClickedIn(creature);
            yield return taskGameAction.AsCoroutine();

            Assert.That(creature.HealthLeft, Is.EqualTo(1));
        }
    }
}
