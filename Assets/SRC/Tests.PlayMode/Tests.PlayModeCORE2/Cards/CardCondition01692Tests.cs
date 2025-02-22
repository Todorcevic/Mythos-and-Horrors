using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardCondition01692Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SwapToken()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01692", investigator);
            Card01692 conditionCard = _cardsProvider.GetCard<Card01692>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator DualCardsInHandSwapToken()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuildCard("01692", investigator);
            yield return BuildCard("01692", investigator);
            IEnumerable<Card01692> conditionCards = _cardsProvider.GetCards<Card01692>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCards, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCards.First());
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Keys.Value, Is.EqualTo(1));
        }
    }
}
