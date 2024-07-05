using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01543Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Drawing()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Investigator investigatorChoosen = _investigatorsProvider.First;
            yield return BuildCard("01543", investigator);
            yield return BuildCard("01507", investigator);
            yield return BuildCard("01596", investigator);
            yield return BuildCard("01603", investigator);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigatorChoosen);

            Card01543 conditionCard = _cardsProvider.GetCard<Card01543>();
            Card01507 cardToDraw1 = _cardsProvider.GetCard<Card01507>();
            Card01596 cardToDraw2 = _cardsProvider.GetCard<Card01596>();
            Card01603 cardToDraw3 = _cardsProvider.GetCard<Card01603>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToDraw1, investigatorChoosen.DeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToDraw2, investigatorChoosen.DeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardToDraw3, investigatorChoosen.DeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();
            int expecetdDeckSize = investigatorChoosen.DeckZone.Cards.Count - 3;

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(investigatorChoosen.AvatarCard);
            yield return ClickedIn(investigatorChoosen.HandZone.Cards.First());
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigatorChoosen.DeckZone.Cards.Count, Is.EqualTo(expecetdDeckSize));
            Assert.That(cardToDraw3.IsInPlay, Is.True);
        }
    }
}
