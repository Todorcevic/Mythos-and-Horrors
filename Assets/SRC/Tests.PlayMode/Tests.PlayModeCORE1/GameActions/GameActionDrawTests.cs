using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Linq;
using System;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionDrawTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator DrawTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);
            Card cardToDraw = _cardsProvider.GetCard<Card01531>();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
              .SetWith(cardToDraw, investigator.DeckZone, isFaceDown: true).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardToDraw);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DeckZone.Cards.Contains(cardToDraw), Is.False);
        }

        [UnityTest]
        public IEnumerator RestoreTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card cardToDraw = _cardsProvider.GetCard<Card01531>();
            Reaction<DrawGameAction> drawReaction = null;
            drawReaction = _reactionablesProvider.CreateReaction<DrawGameAction>(DrawCondition, DrawLogic, GameActionTime.Before);
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(investigator.DeckZone.Cards, investigator.DiscardZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute().AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));

            /*******************************************************************/
            async Task DrawLogic(DrawGameAction action)
            {
                action.SetWith(investigator, cardToDraw);
                _reactionablesProvider.RemoveReaction(drawReaction);
                await Task.CompletedTask;
            }

            bool DrawCondition(DrawGameAction action) => true;
        }


        [UnityTest]
        public IEnumerator Restore2Test()
        {
            Investigator investigator = _investigatorsProvider.First;
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(investigator.DeckZone.Cards.Skip(1), investigator.DiscardZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedIn(investigator.CardAidToDraw);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.DeckZone.Cards.Count, Is.GreaterThan(10));
        }

        [UnityTest]
        public IEnumerator RestoreDangerDeckTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01166 cardAdversity = _cardsProvider.GetCard<Card01166>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(SceneCORE1.DangerDeckZone.Cards, SceneCORE1.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(cardAdversity, SceneCORE1.DangerDiscardZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute().AsCoroutine();

            Assert.That(cardAdversity.CurrentZone, Is.EqualTo(SceneCORE1.DangerDeckZone));
        }

        [UnityTest]
        public IEnumerator RestoreDangerDeckWhenLastIsInLimboTest()
        {
            Investigator investigator = _investigatorsProvider.First;
            Card01564 cardAsset = _cardsProvider.GetCard<Card01564>();
            Card01166 cardAdversity = _cardsProvider.GetCard<Card01166>();
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);

            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
               .SetWith(cardAsset, investigator.HandZone).Execute().AsCoroutine();

            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
              .SetWith(SceneCORE1.DangerDeckZone.Cards, SceneCORE1.OutZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(cardAdversity, SceneCORE1.DangerDeckZone, isFaceDown: true).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(SceneCORE1.DangerCards.Except(new[] { cardAdversity }), SceneCORE1.DangerDiscardZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(cardAsset);
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(SceneCORE1.DangerDeckZone.Cards.Count, Is.GreaterThan(10));
        }
    }
}
