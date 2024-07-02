
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01546Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DrawCardWhenElude()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return BuildCard("01546", investigator);
            Card01546 supply = _cardsProvider.GetCard<Card01546>();
            CardCreature creature = SceneCORE1.GhoulVoraz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value0);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(supply, investigator.AidZone).Start().AsCoroutine();
            int amountCardsInDeck = investigator.DeckZone.Cards.Count;

            Task taskGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Start();
            yield return ClickedClone(creature, 1);
            yield return ClickedMainButton();
            yield return ClickedIn(supply);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Count, Is.EqualTo(amountCardsInDeck - 1));
            Assert.That(creature.Exausted.IsActive, Is.True);
        }
    }
}
