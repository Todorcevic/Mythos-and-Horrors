using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

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
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigatorChoosen);
            int expecetdDeckSize = investigatorChoosen.DeckZone.Cards.Count - 3;
            Card01543 conditionCard = _cardsProvider.GetCard<Card01543>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(conditionCard, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(investigatorChoosen.AvatarCard);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigatorChoosen.DeckZone.Cards.Count, Is.EqualTo(expecetdDeckSize));
        }
    }
}
