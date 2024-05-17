using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.PlayMode.Tests
{

    public class Card01114Tests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator TakeDamageTest()
        {
            CardPlace Cellar = _cardsProvider.GetCard<Card01114>();
            Investigator investigator = _investigatorsProvider.AllInvestigators.First();
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Cellar, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Cellar)).AsCoroutine();

            Assert.That(investigator.DamageRecived, Is.EqualTo(1));
        }
    }
}
