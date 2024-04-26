using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class DrawGameActionTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DrawTest()
        {
            yield return _preparationScene.PlayThisInvestigator(_investigatorsProvider.First);
            Card cardToDraw = _investigatorsProvider.First.CardAidToDraw;

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(cardToDraw);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;
            Assert.That(_investigatorsProvider.First.HandZone.Cards.Contains(cardToDraw), Is.True);
        }
    }
}
