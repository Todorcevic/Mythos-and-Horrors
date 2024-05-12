using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigateGameActionTest : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatePlace()
        {
            MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            Card toPlay = _cardsProvider.GetCard<Card01538>();
            Card toPlay2 = _cardsProvider.GetCard<Card01522>();
            CardPlace place = _cardsProvider.GetCard<Card01113>();

            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();

            Task<PlayInvestigatorGameAction> gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));

            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }
    }
}
