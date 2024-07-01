using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01688Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator AttackAndExtraTurn()
        {
            Investigator investigator = _investigatorsProvider.Third;
            yield return BuildCard("01688", investigator);
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01688 weaponCard = _cardsProvider.GetCard<Card01688>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(weaponCard, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulVoraz, investigator.DangerZone).Start().AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(weaponCard);
            yield return ClickedIn(SceneCORE1.GhoulSecuaz);
            yield return ClickedMainButton();
            Assert.That(investigator.CurrentTurns.Value, Is.EqualTo(3));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulSecuaz.CurrentZone, Is.EqualTo(SceneCORE1.DangerDiscardZone));
            Assert.That(weaponCard.Charge.Amount.Value, Is.EqualTo(2));
            Assert.That(weaponCard.Used.IsActive, Is.True);
        }
    }
}
