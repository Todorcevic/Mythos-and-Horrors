using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardAdversity01182Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator ChallengeToDiscard()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01182 cardAdversity = _cardsProvider.GetCard<Card01182>();
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardAdversity).Start().AsCoroutine();
            Assert.That(investigator.Power.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Power - 1));
            Assert.That(investigator.Sanity.Value, Is.EqualTo(investigator.InvestigatorCard.Info.Sanity - 1));
            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedIn(cardAdversity);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(cardAdversity.IsInPlay, Is.False);
        }
    }
}
