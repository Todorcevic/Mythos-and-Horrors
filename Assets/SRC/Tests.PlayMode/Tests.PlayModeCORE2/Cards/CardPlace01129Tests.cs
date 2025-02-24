
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardPlace01129Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        //[UnityTest]
        //public IEnumerator SearchTomeOrSpell()
        //{
        //    Investigator investigator = _investigatorsProvider.Second;
        //    Card01129 cardPlace = _cardsProvider.GetCard<Card01129>();
        //    Card01531 tome = _cardsProvider.GetCard<Card01531>();
        //    yield return PlaceOnlyScene();
        //    yield return PlayThisInvestigator(investigator);

        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(tome, investigator.DeckZone, true).Execute().AsCoroutine();

        //    Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
        //    yield return ClickedClone(cardPlace, 1);
        //    yield return ClickedIn(tome);
        //    yield return ClickedResourceButton();
        //    yield return ClickedUndoButton();
        //    yield return ClickedMainButton();
        //    yield return gameActionTask.AsCoroutine();

        //    Assert.That(tome.CurrentZone, Is.EqualTo(investigator.HandZone));
        //}

        //[UnityTest]
        //public IEnumerator NoTomeOrSpell()
        //{
        //    Investigator investigator = _investigatorsProvider.Second;
        //    Card01129 cardPlace = _cardsProvider.GetCard<Card01129>();
        //    yield return PlaceOnlyScene();
        //    yield return PlayThisInvestigator(investigator);

        //    yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, cardPlace).Execute().AsCoroutine();
        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator.DeckZone.Cards.Where(card => card.HasThisTag(Tag.Tome) || card.HasThisTag(Tag.Spell)), SceneCORE2.OutZone).Execute().AsCoroutine();

        //    int hadSizeExpeceted = investigator.HandSize;
        //    Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
        //    yield return ClickedClone(cardPlace, 1);
        //    yield return ClickedResourceButton();
        //    yield return ClickedUndoButton();
        //    yield return ClickedMainButton();
        //    yield return gameActionTask.AsCoroutine();

        //    Assert.That(investigator.HandSize, Is.EqualTo(hadSizeExpeceted));
        //}
    }
}
