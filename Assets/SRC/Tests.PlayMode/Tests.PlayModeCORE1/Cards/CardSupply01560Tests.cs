
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01560Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SucessAttack()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01560 weaponCard = _cardsProvider.GetCard<Card01560>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(weaponCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulVoraz, investigator.DangerZone)).AsCoroutine();

            Assert.That(weaponCard.AmountCharges.Value, Is.EqualTo(4));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(weaponCard);
            yield return ClickedIn(SceneCORE1.GhoulVoraz);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(weaponCard.AmountCharges.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator FailAttack()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_1);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01560 weaponCard = _cardsProvider.GetCard<Card01560>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(weaponCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulVoraz, investigator.DangerZone)).AsCoroutine();

            Assert.That(weaponCard.AmountCharges.Value, Is.EqualTo(4));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(weaponCard);
            yield return ClickedIn(SceneCORE1.GhoulVoraz);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(weaponCard.AmountCharges.Value, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator SpecialTokenRevealed()
        {
            Investigator investigator = _investigatorsProvider.First;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Fail);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            Card01560 weaponCard = _cardsProvider.GetCard<Card01560>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(weaponCard, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulSecuaz, investigator.DangerZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE1.GhoulVoraz, investigator.DangerZone)).AsCoroutine();

            Assert.That(weaponCard.AmountCharges.Value, Is.EqualTo(4));

            Task<PlayInvestigatorGameAction> taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(weaponCard);
            yield return ClickedIn(SceneCORE1.GhoulVoraz);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(SceneCORE1.GhoulVoraz.DamageRecived.Value, Is.EqualTo(0));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
            Assert.That(weaponCard.AmountCharges.Value, Is.EqualTo(3));
        }
    }
}
