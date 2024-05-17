using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorCard01504Tests : TestCORE1Preparation
    {
        //protected override bool DEBUG_MODE => true;

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
            yield return _gameActionsProvider.Create(new DecrementStatGameAction(investigatorToTest.Sanity, 3)).AsCoroutine();

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
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, investigatorToTest.CurrentPlace.OwnZone)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigatorToTest, creature, amountFear: 3, isDirect: true));
            yield return ClickedIn(cardInvestigator);
            yield return ClickedIn(creature);
            yield return taskGameAction.AsCoroutine();

            Assert.That(creature.Health.Value, Is.EqualTo(1));
        }
    }
}
