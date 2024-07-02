
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlace01126Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ActivateDrawCards()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01126 cardPlace = _cardsProvider.GetCard<Card01126>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            int DeckSizeExpected = investigator.DeckZone.Cards.Count - 3;

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.GetPlaceZone(0, 3).Cards.First(), SceneCORE2.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlace, SceneCORE2.GetPlaceZone(0, 3)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Count, Is.EqualTo(DeckSizeExpected));
        }
    }
}
