using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionUndoTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator UndoAllInvestigatorDrawTest()
        {
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<AllInvestigatorsDrawCardAndResourceGameAction>().Execute().AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoRestorePhaseGameActionTest()
        {
            yield return PlayAllInvestigators();
            yield return _gameActionsProvider.Create<RestorePhaseGameAction>().Execute().AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.Resources.Value == 0), Is.True);
            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HandZone.Cards.Count == 0), Is.True);
        }

        [UnityTest]
        public IEnumerator UndoTurnsStatTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            Task gameActionTask = _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator UndoWhenDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01518 ally = _cardsProvider.GetCard<Card01518>();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<RevealGameAction>().SetWith(SceneCORE1.Attic).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(ally, investigator.AidZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Third.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Fourth.AvatarCard);
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();
            yield return _gameActionsProvider.Rewind().AsCoroutine();

            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }

        //protected override TestsType TestsType => TestsType.Debug;
        [UnityTest]
        public IEnumerator UndoBackToLastInvestigator()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();

            Task gameActionTask = _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedResourceButton();
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedResourceButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            AssumeThat(investigator.CurrentTurns.Value == 0);
            yield return ClickedIn(_investigatorsProvider.Third.AvatarCard);
            yield return ClickedResourceButton();
            AssumeThat(_investigatorsProvider.Second.CurrentTurns.Value == 3);
            AssumeThat(_investigatorsProvider.Third.CurrentTurns.Value == 2);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Fourth.AvatarCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(_investigatorsProvider.GetInvestigatorsCanStartTurn.Count(), Is.EqualTo(0));
        }
    }
}
