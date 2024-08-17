using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionChooseInvestigatorTest : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChooseInvestigatorTest()
        {
            Card fastCard = _cardsProvider.GetCard<Card01530>();


            yield return StartingScene(withResources: true);
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(_investigatorsProvider.Second.CurrentTurns, 1).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(fastCard, _investigatorsProvider.Second.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<ChooseInvestigatorGameAction>().SetWith().Execute();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedTokenButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.Second.CurrentTurns.Value, Is.EqualTo(0));
        }
    }
}
