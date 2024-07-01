using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01506Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Attack()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_4);
            yield return StartingScene();
            Card01506 weaponCard = _cardsProvider.GetCard<Card01506>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(weaponCard, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulVoraz, investigator.DangerZone).Start().AsCoroutine();

            Assert.That(weaponCard.Charge.Amount.Value, Is.EqualTo(4));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(weaponCard);
            yield return ClickedIn(SceneCORE1.GhoulVoraz);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(weaponCard.Charge.Amount.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NoBullets()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return StartingScene();
            Card01506 weaponCard = _cardsProvider.GetCard<Card01506>();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(weaponCard, investigator.AidZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(weaponCard.Charge.Amount, 0).Start().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulSecuaz, investigator.DangerZone).Start().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE1.GhoulVoraz, investigator.DangerZone).Start().AsCoroutine();

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(weaponCard.Charge.Amount.Value, Is.EqualTo(0));
        }
    }
}
