
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardTalent01567Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator HealthFear()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01567 cardTalent = _cardsProvider.GetCard<Card01567>();
            Card01167 adversity = _cardsProvider.GetCard<Card01167>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardTalent, investigator.HandZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, cardTalent, amountFear: 3)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new DrawGameAction(investigator, adversity));
            yield return ClickedIn(cardTalent);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
        }
    }
}
