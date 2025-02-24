
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlace01127Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        //[UnityTest]
        //public IEnumerator TakeSupport()
        //{
        //    Investigator investigator = _investigatorsProvider.First;
        //    Card01127 cardPlace = _cardsProvider.GetCard<Card01127>();
        //    Card01521 support = _cardsProvider.GetCard<Card01521>();
        //    yield return PlaceOnlyScene();
        //    yield return PlayThisInvestigator(investigator);

        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.GetPlaceZone(0, 3).Cards.First(), SceneCORE2.OutZone).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlace, SceneCORE2.GetPlaceZone(0, 3)).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();

        //    Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
        //    yield return ClickedClone(cardPlace, 1);
        //    yield return ClickedIn(support);
        //    yield return ClickedIn(cardPlace);
        //    yield return ClickedUndoButton();
        //    yield return ClickedMainButton();
        //    yield return gameActionTask.AsCoroutine();

        //    Assert.That(support.CurrentZone, Is.EqualTo(investigator.HandZone));
        //}

        //[UnityTest]
        //public IEnumerator NoSopportInDeck()
        //{
        //    Investigator investigator = _investigatorsProvider.First;
        //    Card01127 cardPlace = _cardsProvider.GetCard<Card01127>();
        //    yield return PlaceOnlyScene();
        //    yield return PlayThisInvestigator(investigator);

        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.GetPlaceZone(0, 3).Cards.First(), SceneCORE2.OutZone).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlace, SceneCORE2.GetPlaceZone(0, 3)).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();

        //    IEnumerable<Card> allSupport = investigator.DeckZone.Cards.Where(card => card.HasThisTag(Tag.Ally));
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(allSupport, SceneCORE2.OutZone).Execute().AsCoroutine();

        //    Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
        //    yield return ClickedClone(cardPlace, 1);
        //    yield return ClickedIn(cardPlace);
        //    yield return ClickedUndoButton();
        //    yield return ClickedMainButton();
        //    yield return gameActionTask.AsCoroutine();

        //    Assert.That(cardPlace.InvestigatorsUsed[investigator].IsActive, Is.True);
        //}
    }
}
