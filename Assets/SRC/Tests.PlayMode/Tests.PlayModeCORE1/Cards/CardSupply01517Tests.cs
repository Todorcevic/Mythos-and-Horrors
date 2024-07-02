
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01517Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator IncrementChallengeSkill()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01517 cardSupply = _cardsProvider.GetCard<Card01517>();
            CardCreature creature = SceneCORE1.GhoulVoraz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(creature, 0);
            yield return ClickedIn(cardSupply);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.DamageRecived.Value, Is.EqualTo(1));
        }
    }
}
