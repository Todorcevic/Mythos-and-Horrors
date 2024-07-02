
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{

    public class CardPlace01124Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SpawnGhoul()
        {
            Investigator investigator = _investigatorsProvider.First;
            CardCreature ghoul = _cardsProvider.GetCard<Card01116>();
            Card01124 home = _cardsProvider.GetCard<Card01124>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(ghoul, investigator).Start().AsCoroutine();

            Assert.That(ghoul.CurrentPlace, Is.EqualTo(home));
        }

        [UnityTest]
        public IEnumerator ActivationDrawCardAndResource()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01124 home = _cardsProvider.GetCard<Card01124>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            int DeckSizeExpected = investigator.DeckZone.Cards.Count - 1;
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, home).Start().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(home, 1);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
            Assert.That(investigator.DeckZone.Cards.Count, Is.EqualTo(DeckSizeExpected));
        }
    }
}
