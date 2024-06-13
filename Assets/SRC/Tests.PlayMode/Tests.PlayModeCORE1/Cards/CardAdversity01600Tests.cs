using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardAdversity01600Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeFearWhenTakeDamageAndPayToDiscard()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01600", investigator);
            Card adversity = _cardsProvider.GetCard<Card01600>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, adversity)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, adversity, amountDamage: 1, isDirect: true)).AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(adversity);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(1));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(adversity.IsInPlay, Is.False);
        }
    }
}
