﻿
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardSupply01576Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator EludeCreature()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01576", investigator);
            Card01576 supply = _cardsProvider.GetCard<Card01576>();
            CardCreature creature = SceneCORE2.Drew;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();

            Assert.That(investigator.AllTypeCreaturesConfronted.Any(), Is.True);
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            yield return ClickedIn(creature);
            Assert.That(investigator.CurrentActions.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(creature.Exausted.IsActive, Is.True);
            Assert.That(investigator.AllTypeCreaturesConfronted.Any(), Is.False);
            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
