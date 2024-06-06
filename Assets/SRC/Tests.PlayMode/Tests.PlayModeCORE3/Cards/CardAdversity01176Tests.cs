using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardAdversity01176Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator FailChallenge()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01176 cardAdversity = _cardsProvider.GetCard<Card01176>();
            Card01600 madness = _cardsProvider.GetCard<Card01600>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);

            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(madness, investigator.DeckZone, isFaceDown: true)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ChangeCardPositionGameAction(madness, 10)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new DrawGameAction(investigator, cardAdversity));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
            Assert.That(investigator.DangerZone.Cards.Contains(madness), Is.True);
        }
    }
}
