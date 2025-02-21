using MythosAndHorrors.GameRules;
using System.Linq;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionSafeForeachTest : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SafeForeachBasic()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            IEnumerable<Investigator> AllInvestigatorsInStudy() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == SceneCORE1.Study);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInStudy, DiscardAndMoveInvestigatorToStudy).Execute().AsCoroutine();

            Assert.That(investigator.HandSize, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator SafeForeachMutable()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            IEnumerable<Investigator> AllInvestigatorsInStudy() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == SceneCORE1.Study);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInStudy, DiscardSelectionAndMoveInvestigatorToStudy).Execute();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.Third, SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return ClickedIn(_investigatorsProvider.Second.HandZone.Cards.First());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(_investigatorsProvider.Fourth.HandZone.Cards.First());
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.HandSize, Is.EqualTo(4));
            Assert.That(_investigatorsProvider.Third.HandSize, Is.EqualTo(5));
        }

        [UnityTest]
        public IEnumerator SafeForeachWithSelection()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return StartingScene();
            IEnumerable<Investigator> AllInvestigatorsInStudy() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == SceneCORE1.Study);
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE1.Hallway).Execute().AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInStudy, DiscardSelectionAndMoveInvestigatorToStudy).Execute();
            yield return ClickedIn(_investigatorsProvider.Second.HandZone.Cards.First());
            yield return ClickedIn(investigator.HandZone.Cards.First());
            yield return ClickedIn(_investigatorsProvider.Third.HandZone.Cards.First());
            yield return ClickedIn(_investigatorsProvider.Fourth.HandZone.Cards.First());
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.HandSize, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator SafeForeachUndo()
        {
            Investigator investigator = _investigatorsProvider.First;
            SafeForeachReaction();
            yield return StartingScene();
            yield return _gameActionsProvider.Create<RevealGameAction>().SetWith(SceneCORE1.Hallway).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, SceneCORE1.Hallway).Execute().AsCoroutine();
            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedResourceButton();
            yield return ClickedResourceButton();
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.HandZone.Cards.First());
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            AssumeThat(investigator.CurrentActions.Value == 1);
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.HandSize, Is.EqualTo(5));
            Assert.That(investigator.Resources.Value, Is.EqualTo(3));
        }

        /*******************************************************************/
        private void SafeForeachReaction()
        {
            Reaction<PlayInvestigatorGameAction> reaction = null;
            reaction = _reactionablesProvider.CreateReaction<PlayInvestigatorGameAction>(Condition, SafeForeachReac, GameActionTime.After);

            bool Condition(GameAction _) => true;

            /*******************************************************************/
            async Task SafeForeachReac(PlayInvestigatorGameAction gameAction)
            {
                _reactionablesProvider.RemoveReaction(reaction);
                await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigators, DiscardSelectionAndMoveInvestigatorToStudy).Execute();
            }

            IEnumerable<Investigator> AllInvestigators() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == SceneCORE1.Study);
        }

        private async Task DiscardAndMoveInvestigatorToStudy(Investigator investigator)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(investigator.HandZone.Cards.First()).Execute();
            await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE1.Study).Execute();
        }

        private async Task DiscardSelectionAndMoveInvestigatorToStudy(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, new Localization("Interactable_TestsPurpose"));

            foreach (Card card in investigator.HandZone.Cards)
            {
                interactableGameAction.CreateCardEffect(card, new Stat(0, false), Activate, PlayActionType.Choose, investigator, new Localization("CardEffect_TestsPurpose"));

                /*******************************************************************/
                async Task Activate()
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.First, SceneCORE1.Study).Execute();
                }

            }
            await interactableGameAction.Execute();
        }
    }
}
