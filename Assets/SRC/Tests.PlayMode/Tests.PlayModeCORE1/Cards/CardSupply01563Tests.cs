
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{

    public class CardSupply01563Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DrawSpell()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            Card01563 supply = _cardsProvider.GetCard<Card01563>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            yield return ClickedIn(supply);
            yield return ClickedIn(investigator.DeckZone.Cards.First(card => card.HasThisTag(Tag.Spell)));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Eldritch.Value, Is.EqualTo(1));
            Assert.That(supply.Exausted.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator NoSpell()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            Card01563 supply = _cardsProvider.GetCard<Card01563>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(supply, investigator.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.DeckZone.Cards.Where(card => card.HasThisTag(Tag.Spell)), SceneCORE1.OutZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            yield return ClickedIn(supply);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(supply.Eldritch.Value, Is.EqualTo(1));
            Assert.That(supply.Exausted.IsActive, Is.True);
        }
    }
}
