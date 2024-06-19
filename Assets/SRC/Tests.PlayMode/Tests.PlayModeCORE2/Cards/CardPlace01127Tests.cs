
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

        [UnityTest]
        public IEnumerator TakeSupport()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01127 cardPlace = _cardsProvider.GetCard<Card01127>();
            Card01521 support = _cardsProvider.GetCard<Card01521>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE2.GetPlaceZone(0, 3).Cards.First(), SceneCORE2.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlace, SceneCORE2.GetPlaceZone(0, 3))).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedIn(support);
            yield return ClickedIn(cardPlace);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(support.CurrentZone, Is.EqualTo(investigator.HandZone));
        }

        [UnityTest]
        public IEnumerator NoSopportInDeck()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01127 cardPlace = _cardsProvider.GetCard<Card01127>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE2.GetPlaceZone(0, 3).Cards.First(), SceneCORE2.OutZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlace, SceneCORE2.GetPlaceZone(0, 3))).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();

            IEnumerable<Card> allSupport = investigator.DeckZone.Cards.Where(card => card.HasThisTag(Tag.Ally));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(allSupport, SceneCORE2.OutZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedIn(cardPlace);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardPlace.InvestigatorsUsed[investigator].IsActive, Is.True);
        }
    }
}
