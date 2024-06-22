
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01520Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Attack()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01520 cardSupply = _cardsProvider.GetCard<Card01520>();
            CardCreature cardCreature = SceneCORE1.GhoulVoraz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2);
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardSupply, investigator.AidZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardCreature, investigator.DangerZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(cardCreature);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(cardCreature.DamageRecived.Value, Is.EqualTo(2));
        }
    }
}
