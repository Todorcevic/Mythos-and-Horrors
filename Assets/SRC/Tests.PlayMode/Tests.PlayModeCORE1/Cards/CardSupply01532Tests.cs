
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardSupply01532Tests : TestCORE1Preparation
    {
        protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator TakeTomeWheSpawn()
        {
            Investigator investigator = _investigatorsProvider.Second;
            Card01532 cardSupply = _cardsProvider.GetCard<Card01532>();
            Card01531 tome = _cardsProvider.GetCard<Card01531>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSupply, investigator.HandZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(cardSupply);
            yield return ClickedIn(tome);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(tome.CurrentZone, Is.EqualTo(investigator.HandZone));
            Assert.That(investigator.DeckZone.Cards.All(card => card.FaceDown.IsActive), Is.True);
        }
    }
}
