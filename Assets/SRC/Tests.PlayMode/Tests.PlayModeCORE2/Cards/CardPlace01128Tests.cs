using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{

    public class CardPlace01128Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HealthDamage()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01128 cardPlace = _cardsProvider.GetCard<Card01128>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, cardPlace)).AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, cardPlace, amountDamage: 2)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(0));
        }
    }
}

