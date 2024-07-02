
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01518Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DoDamageAndDiscard()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01518 cardSupply = _cardsProvider.GetCard<Card01518>();
            CardCreature creature = SceneCORE1.GhoulVoraz;
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
            Assert.That(cardSupply.CurrentZone, Is.EqualTo(investigator.DiscardZone));
        }
    }
}
