using ModestTree;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
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

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PushTokenTest()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);

            do
            {
                if (DEBUG_MODE) yield return PressAnyKey();
                yield return _challengeBagComponent.DropToken(new ChallengeToken(ChallengeTokenType.Ancient, () => -2)).AsCoroutine();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(true);
        }

        [UnityTest]
        public IEnumerator FullChallengeTest()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
            Card cardToChallenge = _investigatorsProvider.First.FullDeck.ElementAt(4);
            Card toPlay = _cardsProvider.GetCard("01520");
            Card toPlay2 = _cardsProvider.GetCard("01521");

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_cardsProvider.GetCard<CardPlace>("01112"), _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.Leader, _cardsProvider.GetCard<CardPlace>("01112"))).AsCoroutine();


            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(toPlay2, _investigatorsProvider.Leader.HandZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new ChallengePhaseGameAction(_investigatorsProvider.Leader.Strength, 3, "TestChallenge", SuccessEffect, FailEffect, cardToChallenge)).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(true);
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
