using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01599Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeDamageWhenTakeFearAndPayToDiscard()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01599", investigator);
            Card adversity = _cardsProvider.GetCard<Card01599>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, adversity).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, adversity, amountFear: 1, isInevitable: true).Execute().AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(adversity);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(1));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(adversity.IsInPlay, Is.False);
        }
    }
}
