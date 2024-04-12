using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class InvestigateGameActionTest : TestBase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ReactionableControl _reactionableControl;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator InvestigatePlace()
        {
            _reactionableControl.SubscribeAtStart(RevealMinus1Token);

            yield return PlayThisInvestigator(_investigatorsProvider.First);
            Card toPlay = _cardsProvider.GetCard("01538");
            Card toPlay2 = _cardsProvider.GetCard("01522");
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01113");

            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();

            Task<OneInvestigatorTurnGameAction> gameActionTask =
                _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(_investigatorsProvider.First));

            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            while (!gameActionTask.IsCompleted) yield return null;

            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(2));
            Assert.That(_investigatorsProvider.First.Hints.Value, Is.EqualTo(1));
        }

        private async Task RevealMinus1Token(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            ChallengeToken minus1Token = _challengeTokensProvider.ChallengeTokensInBag
                .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Value_1);
            revealChallengeTokenGameAction.SetChallengeToken(minus1Token);

            await Task.CompletedTask;
        }
    }
}
