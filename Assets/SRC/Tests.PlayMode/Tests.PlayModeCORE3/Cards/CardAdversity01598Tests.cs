using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{


    public class CardAdversity01598Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DecrementStatInPlay()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01598 cardAdversity = _cardsProvider.GetCard<Card01598>();
            yield return StartingScene();

            yield return _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity)).AsCoroutine();
            Assert.That(investigator.Strength.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Strength - 1));

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardAdversity);
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(1));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Strength.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Strength));
            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE3.OutZone));
        }
    }
}
