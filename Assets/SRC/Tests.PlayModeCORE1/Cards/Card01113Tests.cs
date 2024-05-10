using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01113Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator TakeFearOneInvestigatorTest()
        {
            CardPlace Attick = _cardsProvider.GetCard<Card01113>();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationSceneCORE1.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Attick, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Attick)).AsCoroutine();

            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TakeFearAllInvestigatorTest()
        {
            CardPlace Attick = _cardsProvider.GetCard<Card01113>();
            yield return _preparationSceneCORE1.PlayAllInvestigators();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Attick, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigatorsInPlay, Attick)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.FearRecived, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Second.FearRecived, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Third.FearRecived, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.Fourth.FearRecived, Is.EqualTo(1));
        }
    }
}
