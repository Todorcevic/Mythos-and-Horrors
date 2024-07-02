
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01534Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator IncrementChallengeSkill()
        {
            Investigator investigator = _investigatorsProvider.First; //2 Agi
            Card01534 cardSupply = _cardsProvider.GetCard<Card01534>();
            CardCreature creature = SceneCORE1.GhoulVoraz; // 3 Agi
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.AidZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(creature, 1);
            yield return ClickedIn(cardSupply); //+1 Agi
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(creature.Exausted.IsActive, Is.True);
        }
    }
}
