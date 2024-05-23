using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionUpdateStatTests : TestCORE1Preparation
    {
        [UnityTest]
        public IEnumerator GainAndPayHints()
        {
            CardGoal cardGoal = SceneCORE1.FirstGoal;
            Investigator investigator = _investigatorsProvider.First;
            CardPlace place = _cardsProvider.GetCard<Card01112>();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardGoal, SceneCORE1.GoalZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.InvestigatorCard, investigator.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, SceneCORE1.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new RevealGameAction(place)).AsCoroutine();

            yield return _gameActionsProvider.Create(new UpdateStatGameAction(place.Hints, 3)).AsCoroutine();
            yield return _gameActionsProvider.Create(new GainHintGameAction(investigator, place.Hints, 2)).AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(2));

            yield return _gameActionsProvider.Create(new PayHintGameAction(investigator, cardGoal.Hints, 1)).AsCoroutine();

            Assert.That(cardGoal.Hints.Value, Is.EqualTo(7));
            Assert.That(place.Hints.Value, Is.EqualTo(1));
            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }
    }
}
