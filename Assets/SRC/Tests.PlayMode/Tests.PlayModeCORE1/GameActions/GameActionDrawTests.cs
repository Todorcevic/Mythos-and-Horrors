using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
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
            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardToDraw);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Contains(cardToDraw), Is.False);
        }

        //[UnityTest]
        //public IEnumerator RestoreTest()
        //{
        //    Investigator investigator = _investigatorsProvider.First;
        //    yield return PlayThisInvestigator(investigator);

        //    yield return _gameActionsProvider.Create<MoveCardsGameAction>()
        //        .SetWith(investigator.DeckZone.Cards, investigator.DiscardZone).Execute();

        //    yield return _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute().AsCoroutine();

        //    Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        //}
    }
}
