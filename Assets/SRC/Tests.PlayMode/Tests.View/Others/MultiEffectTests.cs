﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class MultiEffectTests : PlayModeTestsBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MultiEffect_Test()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Multieffect test");
            Investigator investigator1 = _investigatorsProvider.First;
            Card card = investigator1.FullDeck[1];
            Card card2 = investigator1.FullDeck[2];

            interactableGameAction.CreateMainButton()
                     .SetLogic(() => Task.CompletedTask);

            interactableGameAction.Create()
                  .SetCard(card)
                  .SetInvestigator(investigator1)
                  .SetLogic(() => _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator1.DangerZone)));

            interactableGameAction.Create()
                .SetCard(card)
                .SetInvestigator(investigator1)
                .SetLogic(() => _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator1.HandZone)));

            interactableGameAction.Create()
                .SetCard(card2)
                .SetInvestigator(investigator1)
                .SetLogic(() => _gameActionsProvider.Create(new MoveCardsGameAction(card2, investigator1.DangerZone)));
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.FullDeck.Take(5).ToList(), investigator1.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(interactableGameAction);

            if (!DEBUG_MODE) yield return WaitToClick(card);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator1.DangerZone.TopCard, Is.EqualTo(card));
        }
    }
}