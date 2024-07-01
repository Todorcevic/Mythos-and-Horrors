using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01512Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DrawWhenPlayASpell()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardSupply hiperborea = _cardsProvider.GetCard<Card01512>();
            Card01560 spell = _cardsProvider.GetCard<Card01560>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card card = investigator.CardAidToDraw;
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(hiperborea, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(spell, investigator.HandZone).Start().AsCoroutine();

            Assert.That(card.CurrentZone, Is.EqualTo(investigator.DeckZone));
            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(spell);
            yield return ClickedIn(hiperborea);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(card.CurrentZone, Is.Not.EqualTo(investigator.DeckZone));
        }

    }
}
