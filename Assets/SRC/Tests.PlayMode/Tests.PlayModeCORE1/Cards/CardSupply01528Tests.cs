
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01528Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DoDamageAndExhaust()
        {
            Investigator investigator = _investigatorsProvider.Second;
            CardCreature creature = SceneCORE1.GhoulVoraz;
            yield return BuildCard("01528", investigator);
            Card01528 cardSupply = _cardsProvider.GetCard<Card01528>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();

            Assert.That(investigator.Strength.Value, Is.EqualTo(3));

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(creature);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(cardSupply.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(cardSupply.Exausted.IsActive, Is.True);
        }
    }
}
