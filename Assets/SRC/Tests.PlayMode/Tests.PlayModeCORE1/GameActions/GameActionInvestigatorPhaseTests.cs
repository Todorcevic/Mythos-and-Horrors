using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class UndoInvestigatorPhaseWithTests : TestCORE1Preparation
    {
        protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SecondPlayerUndoToFirstPlayer()
        {
            yield return StartingScene(withResources: true, withAvatar: true);
            Investigator investigator = _investigatorsProvider.First;

            Task taskGameAction = _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedResourceButton();
            yield return ClickedResourceButton();
            yield return ClickedResourceButton();
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedResourceButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Third.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Fourth.AvatarCard);
            yield return ClickedMainButton();
            yield return taskGameAction.AsCoroutine();

            Assert.That(investigator.Resources.Value, Is.EqualTo(6));
        }

        [UnityTest]
        public IEnumerator UndoSharedDamage()
        {
            yield return StartingScene(withResources: true, withAvatar: true);
            Investigator investigator = _investigatorsProvider.First;
            yield return BuildCard("01594", investigator);
            CardCreature cardCreature = _cardsProvider.GetCard<Card01119>();
            Card01594 bulletProof = _cardsProvider.GetCard<Card01594>();
            Card01521 damageableCard = _cardsProvider.GetCard<Card01521>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardCreature, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(damageableCard, investigator.AidZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(bulletProof, investigator.AidZone).Execute().AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedResourceButton();
            yield return ClickedIn(bulletProof);
            yield return ClickedUndoButton();
            yield return ClickedUndoButton();
            yield return ClickedIn(_investigatorsProvider.Second.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Third.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(_investigatorsProvider.Fourth.AvatarCard);
            yield return ClickedMainButton();
            yield return ClickedIn(investigator.AvatarCard);
            yield return ClickedMainButton();

            yield return taskGameAction.AsCoroutine();

            Assert.That(_investigatorsProvider.AllInvestigatorsInPlay.All(investigator => investigator.HasTurnsAvailable.IsFalse), Is.True);
        }
    }
}
