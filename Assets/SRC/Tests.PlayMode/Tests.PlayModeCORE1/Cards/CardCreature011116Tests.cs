using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCreature011116Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator CounterAttack()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator, withResources: true);
            Card01116 creature = _cardsProvider.GetCard<Card01116>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, SceneCORE1.Study.OwnZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(creature, 0);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }
    }
}
