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
        public IEnumerator TakeFearTest()
        {
            CardPlace Attick = _cardsProvider.GetCard<Card01113>();
            Investigator investigator = _investigatorsProvider.AllInvestigators.First();
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Attick, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Attick)).AsCoroutine();

            Assert.That(investigator.FearRecived, Is.EqualTo(1));
        }
    }
}
