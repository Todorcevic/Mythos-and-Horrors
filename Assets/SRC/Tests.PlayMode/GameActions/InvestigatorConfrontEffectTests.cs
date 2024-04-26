using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigatorConfrontEffectTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatorConfrontTest()
        {
            CardCreature creature = _preparationScene.SceneCORE1.GhoulSecuaz;
            yield return _preparationScene.StartingScene();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creature, _investigatorsProvider.First.CurrentPlace.OwnZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));

            if (!DEBUG_MODE) yield return WaitToClick(creature);
            if (!DEBUG_MODE) yield return WaitToCloneClick(1);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(creature.CurrentZone, Is.EqualTo(_investigatorsProvider.First.DangerZone));
        }
    }
}
