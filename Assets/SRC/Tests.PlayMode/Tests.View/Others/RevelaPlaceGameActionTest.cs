using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    public class RevelaPlaceGameActionTest : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator RevealPlaceTest()
        {
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.GetPlaceZone(0, 4))).AsCoroutine();

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.First, place)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That((_cardViewsManager.GetCardView(place) as PlaceCardView).GetPrivateMember<StatView>("_hints").isActiveAndEnabled);
        }
    }
}
