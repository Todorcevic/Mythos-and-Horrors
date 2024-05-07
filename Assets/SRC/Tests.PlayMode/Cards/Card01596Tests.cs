using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01596Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ObligationTest()
        {
            Card adversity = _cardLoaderUseCase.Execute("01596");
            _cardViewGeneratorComponent.BuildCardView(adversity);
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlaceAllScene();
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new AddRequerimentCardGameAction(investigator, adversity)).AsCoroutine();

            Card cardToMaintan = investigator.HandZone.Cards.First(card => card.CanDiscard);

            Task gameActionTask = _gameActionsProvider.Create(new DrawGameAction(investigator, adversity));

            if (!DEBUG_MODE) yield return WaitToClick(cardToMaintan);

            while (!gameActionTask.IsCompleted) yield return null;

            Assert.That(_investigatorsProvider.First.HandZone.Cards.Count, Is.EqualTo(1));
            Assert.That(_investigatorsProvider.First.HandZone.Cards.Unique(), Is.EqualTo(cardToMaintan));
            investigator.RequerimentCard.Remove(adversity);
        }

    }
}
