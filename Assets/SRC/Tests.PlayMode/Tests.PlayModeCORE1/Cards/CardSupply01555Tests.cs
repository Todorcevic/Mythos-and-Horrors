
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01555Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator UnconfrontAndMove()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return BuildCard("01555", investigator);
            Card01555 supply = _cardsProvider.GetCard<Card01555>();
            CardCreature creature = SceneCORE1.GhoulVoraz;
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, SceneCORE1.Hallway)).AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(supply);
            yield return AssertThatIsNotClickable(SceneCORE1.Parlor);
            yield return ClickedIn(SceneCORE1.Attic);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE1.Attic));
            Assert.That(creature.CurrentPlace, Is.EqualTo(SceneCORE1.Hallway));
            Assert.That(supply.Exausted.IsActive, Is.True);
        }
    }
}
