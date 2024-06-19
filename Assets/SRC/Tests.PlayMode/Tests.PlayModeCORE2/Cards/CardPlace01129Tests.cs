
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

        [UnityTest]
        public IEnumerator SearchTomeOrSpell()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01129 cardPlace = _cardsProvider.GetCard<Card01129>();
            Card01531 tome = _cardsProvider.GetCard<Card01531>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(tome, investigator.DeckZone, true)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedIn(tome);
            yield return ClickedIn(cardPlace);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(tome.CurrentZone, Is.EqualTo(investigator.HandZone));
        }

        [UnityTest]
        public IEnumerator NoTomeOrSpell()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01129 cardPlace = _cardsProvider.GetCard<Card01129>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.DeckZone.Cards.Where(card => card.HasThisTag(Tag.Tome) || card.HasThisTag(Tag.Spell)), SceneCORE2.OutZone)).AsCoroutine();

            int hadSizeExpeceted = investigator.HandSize;
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedIn(cardPlace);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.HandSize, Is.EqualTo(hadSizeExpeceted));
        }
    }
}
