using MythosAndHorrors.GameRules;
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
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Interactable_TestsPurpose");
            Investigator investigator1 = _investigatorsProvider.First;
            Card card = investigator1.FullDeck[1];
            Card card2 = investigator1.FullDeck[2];

            interactableGameAction.CreateContinueMainButton();
            interactableGameAction.CreateCardEffect(card, new Stat(0, false), () => _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, investigator1.DangerZone).Execute(),
                PlayActionType.Choose, investigator1, "CardEffect_TestsPurpose");
            interactableGameAction.CreateCardEffect(card, new Stat(0, false), () => _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, investigator1.HandZone).Execute(),
                PlayActionType.Choose, investigator1, "CardEffect_TestsPurpose");
            interactableGameAction.CreateCardEffect(card2, new Stat(0, false), () => _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card2, investigator1.DangerZone).Execute(),
                PlayActionType.Choose, investigator1, "CardEffect_TestsPurpose");
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(investigator1.FullDeck.Take(5).ToList(), investigator1.HandZone).Execute().AsCoroutine();

            Task gameActionTask = interactableGameAction.Execute();

            if (!DEBUG_MODE) yield return WaitToClick(card);
            if (!DEBUG_MODE) yield return WaitToCloneClick(0);

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator1.DangerZone.TopCard, Is.EqualTo(card));
        }
    }
}
