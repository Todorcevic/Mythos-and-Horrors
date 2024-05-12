using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class DrawGameActionTests : TestCORE1PlayModeBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator DrawTest()
        {
            yield return _preparationSceneCORE1.PlayThisInvestigator(_investigatorsProvider.First);
            Card cardToDraw = _investigatorsProvider.First.CardAidToDraw;

            Assert.That(_investigatorsProvider.First.DeckZone.Cards.Contains(cardToDraw), Is.True);
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(_investigatorsProvider.First));
            if (!DEBUG_MODE) yield return WaitToClick(cardToDraw);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return gameActionTask.AsCoroutine();
            Assert.That(_investigatorsProvider.First.DeckZone.Cards.Contains(cardToDraw), Is.False);
        }
    }
}
