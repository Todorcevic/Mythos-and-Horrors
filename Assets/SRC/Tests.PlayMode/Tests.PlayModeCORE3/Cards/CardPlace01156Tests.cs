
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlace01156Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ResetHints()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01156 cardPlace = _cardsProvider.GetCard<Card01156>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.Forests[2], SceneCORE3.OutZone).Execute();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlace, SceneCORE3.GetPlaceZone(2, 2)).Execute();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<RoundGameAction>().Execute();
            yield return ClickedIn(cardPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
            Assert.That(cardPlace.Hints.Value, Is.EqualTo(8));
        }
    }
}
