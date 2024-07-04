
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01687Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator UseAndBreak()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return BuildCard("01687", investigator);
            Card01687 cardAsset = _cardsProvider.GetCard<Card01687>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardAsset, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(cardAsset.Charge.Amount, 1).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardAsset);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(cardAsset.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
