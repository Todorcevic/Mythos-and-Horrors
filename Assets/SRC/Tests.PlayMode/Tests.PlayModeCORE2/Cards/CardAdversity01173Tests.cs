using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{
    public class CardAdversity01173Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator FailMoveInvestigator()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01173 cardAdversity = _cardsProvider.GetCard<Card01173>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE2.Drew, investigator.DangerZone).Start().AsCoroutine();
            Task<DrawGameAction> taskGameAction = _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.CurrentPlace, Is.EqualTo(SceneCORE2.Center));
            Assert.That(SceneCORE2.Drew.CurrentPlace, Is.EqualTo(SceneCORE2.Fluvial));
            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}