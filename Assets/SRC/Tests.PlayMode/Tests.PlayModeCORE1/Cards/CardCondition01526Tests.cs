using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01526Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Reload()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Third;

            yield return BuildCard("01526", investigator2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);
            Card01526 conditionCard = _cardsProvider.GetCard<Card01526>();
            Card01506 weapon = _cardsProvider.GetCard<Card01506>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator2.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(weapon, investigator.AidZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator2));
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(weapon);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(weapon.Charge.Amount.Value, Is.EqualTo(7));
        }
    }
}
