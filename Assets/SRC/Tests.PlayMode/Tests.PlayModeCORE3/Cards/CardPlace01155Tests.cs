
using MythosAndHorrors.GameRules;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayModeCORE3.Tests
{
    public class CardPlace01155Tests : TestCORE3Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator Health()
        {
            Investigator investigator = _investigatorsProvider.First;
            Investigator investigator2 = _investigatorsProvider.Second
;
            Card01155 cardPlace = _cardsProvider.GetCard<Card01155>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return PlayThisInvestigator(investigator2);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.Forests[2], SceneCORE3.OutZone).Execute();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardPlace, SceneCORE3.GetPlaceZone(2, 2)).Execute();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(new[] { investigator, investigator2 }, cardPlace).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, cardPlace, amountDamage: 2, amountFear: 2).Execute();
            yield return _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator2, cardPlace, amountDamage: 2, amountFear: 2).Execute();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedClone(cardPlace, 1);
            yield return ClickedIn(cardPlace);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();
            gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator2).Execute();
            yield return ClickedClone(cardPlace, 2);
            yield return ClickedIn(cardPlace);
            yield return ClickedUndoButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(2));
            Assert.That(investigator2.DamageRecived.Value, Is.EqualTo(2));
            Assert.That(investigator2.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
