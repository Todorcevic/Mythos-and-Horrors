using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChallengeTests : TestBase
    {
        [Inject] private readonly ChallengeBagComponent _challengeBagComponent;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PushTokenTest()
        {
            ChallengeToken challengeToken = new(ChallengeTokenType.Ancient, () => -2);
            yield return PlayThisInvestigator(_investigatorsProvider.First);

            do
            {
                if (DEBUG_MODE) yield return PressAnyKey();

                yield return _challengeBagComponent.DropToken(challengeToken).AsCoroutine();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Unique().ChallengeToken,
                Is.EqualTo(challengeToken));

            yield return _challengeBagComponent.RestoreToken(challengeToken).WaitForCompletion();

            Assert.That(_challengeBagComponent.GetPrivateMember<List<ChallengeTokenView>>("_allTokensDrop").Count, Is.EqualTo(0));
        }

        //[UnityTest]
        //public IEnumerator FullChallengeTest()
        //{
        //    yield return PlayThisInvestigator(_investigatorsProvider.First);
        //    Card cardToChallenge = _investigatorsProvider.First.FullDeck.ElementAt(4);
        //    Card toPlay = _cardsProvider.GetCard("01520");
        //    Card toPlay2 = _cardsProvider.GetCard("01521");

        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(_cardsProvider.GetCard<CardPlace>("01112"), _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, _cardsProvider.GetCard<CardPlace>("01112"))).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
        //    yield return _gameActionsProvider.Create(new ChallengePhaseGameAction(_investigatorsProvider.Leader.Strength, 3, "TestChallenge", SuccessEffect, FailEffect, cardToChallenge)).AsCoroutine();


        //    if (DEBUG_MODE) yield return new WaitForSeconds(230);
        //    Assert.That(true);
        //}


        [UnityTest]
        public IEnumerator FullChallengeTestUndo()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            Card toPlay = _cardsProvider.GetCard("01538");
            Card toPlay2 = _cardsProvider.GetCard("01522");
            CardPlace place = _cardsProvider.GetCard<CardPlace>("01113");

            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, GameValues.DEFAULT_TURNS_AMOUNT)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, place)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();

            OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction = new(_investigatorsProvider.First);
            _ = _gameActionsProvider.Create(oneInvestigatorTurnGameAction);

            if (!DEBUG_MODE) yield return WaitToClick(place);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_investigatorsProvider.First.CurrentTurns.Value, Is.EqualTo(2));
        }


        private async Task SuccessEffect()
        {
            await _gameActionsProvider.Create(new DrawAidGameAction(_investigatorsProvider.Leader));
        }

        private async Task FailEffect()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(_investigatorsProvider.Leader.Health, 1));
        }

    }
}
