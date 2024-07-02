
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01542Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator IncrementSkill()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Investigator investigator2 = _investigatorsProvider.First;
            yield return BuildCard("01542", investigator);
            Card01542 supply = _cardsProvider.GetCard<Card01542>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(supply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedClone(investigator2.InvestigatorCard, 0, true);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator2.Strength.Value, Is.EqualTo(6));
            Assert.That(supply.Exausted.IsActive, Is.True);
        }

        [UnityTest]
        public IEnumerator Reset()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Investigator investigator2 = _investigatorsProvider.First;
            yield return BuildCard("01542", investigator);
            Card01542 supply = _cardsProvider.GetCard<Card01542>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedIn(supply);
            yield return ClickedIn(investigator2.InvestigatorCard);
            yield return ClickedClone(investigator2.InvestigatorCard, 0, true);
            Assert.That(investigator2.Strength.Value, Is.EqualTo(6));
            yield return ClickedMainButton();
            yield return ClickedIn(investigator2.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator2.Strength.Value, Is.EqualTo(4));
            Assert.That(supply.Exausted.IsActive, Is.True);
        }
    }
}
