﻿//using MythosAndHorrors.GameRules;
//using NUnit.Framework;
//using System.Collections;
//using System.Threading.Tasks;
//using UnityEngine.TestTools;
//using Zenject;

//namespace MythosAndHorrors.PlayMode.Tests
//{
//    public class Card01505Tests : TestBase
//    {
//        [Inject] private readonly GameActionsProvider _gameActionsProvider;
//        [Inject] private readonly CardsProvider _cardsProvider;
//        [Inject] private readonly ChaptersProvider _chaptersProvider;
//        [Inject] private readonly ReactionableControl _reactionableControl;
//        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

//        //protected override bool DEBUG_MODE => true;

//        /*******************************************************************/
//        [UnityTest]
//        public IEnumerator Investigator5StarToken()
//        {
//            _reactionableControl.SubscribeAtStart(RevealStarToken);
//            CardPlace place = _cardsProvider.GetCard<CardPlace>("01114"); //Enigma:4, Hints: 2

//            CardInvestigator cardInvestigator = _cardsProvider.GetCard<CardInvestigator>("01505");
//            Investigator investigatorToTest = cardInvestigator.Owner;
//            yield return PlayThisInvestigator(investigatorToTest);
//            yield return _gameActionsProvider.Create(new UpdateStatGameAction(investigatorToTest.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
//            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
//            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigatorToTest, place)).AsCoroutine();

//            Task<OneInvestigatorTurnGameAction> taskInvestigator =
//                _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(investigatorToTest));
//            if (!DEBUG_MODE) yield return WaitToClick(place);
//            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

//            while (!taskInvestigator.IsCompleted) yield return null;
//            Assert.That(investigatorToTest.Hints.Value, Is.EqualTo(1));
//        }

//        private async Task RevealStarToken(GameAction gameAction)
//        {
//            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
//            ChallengeToken starToken = _challengeTokensProvider.ChallengeTokensInBag
//               .Find(challengeToken => challengeToken.TokenType == ChallengeTokenType.Star);
//            revealChallengeTokenGameAction.SetChallengeToken(starToken);

//            await Task.CompletedTask;
//        }
//    }
//}