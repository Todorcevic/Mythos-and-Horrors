using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE2.Tests
{

    public class CardCreature01172Tests : TestCORE2Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DoubleTokenValueWithRetry()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01172 creature = _cardsProvider.GetCard<Card01172>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(creature, 1);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedIn(investigator.HandZone.Cards.First(card => card.CanBeDiscarted.IsTrue));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(creature.CurrentZone, Is.EqualTo(investigator.DangerZone));
            Assert.That(_challengeTokensProvider.ChallengeTokensInBag.Count(token => token.Value.Invoke(investigator) == -4), Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator DoubleTokenValue()
        {
            Investigator investigator = _investigatorsProvider.Fourth;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01172 creature = _cardsProvider.GetCard<Card01172>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(creature, 1);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(creature.CurrentZone, Is.EqualTo(investigator.DangerZone));
            Assert.That(_challengeTokensProvider.ChallengeTokensInBag.Count(token => token.Value.Invoke(investigator) == -4), Is.EqualTo(1));
        }
    }
}
