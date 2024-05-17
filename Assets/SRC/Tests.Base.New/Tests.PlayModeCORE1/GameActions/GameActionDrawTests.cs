using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class GameActionDrawTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator DrawTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            Card cardToDraw = investigator.CardAidToDraw;

            Assert.That(investigator.DeckZone.Cards.Contains(cardToDraw), Is.True);
            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardToDraw);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Contains(cardToDraw), Is.False);
        }
    }
}
