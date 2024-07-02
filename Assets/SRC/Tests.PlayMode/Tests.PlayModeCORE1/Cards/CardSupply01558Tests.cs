
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01558Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator GainResources()
        {
            Investigator investigator = _investigatorsProvider.Third;
            Card01558 supply = _cardsProvider.GetCard<Card01558>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(supply);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
            Assert.That(supply.Exausted.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator Discard()
        {
            Investigator investigator = _investigatorsProvider.Third;
            Card01558 supply = _cardsProvider.GetCard<Card01558>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(supply.Charge.Amount, 1).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(supply);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
            Assert.That(supply.Exausted.IsActive, Is.False);
            Assert.That(supply.CurrentZone, Is.EqualTo(investigator.DiscardZone));
            Assert.That(supply.Charge.Amount.Value, Is.EqualTo(4));
        }

    }
}
