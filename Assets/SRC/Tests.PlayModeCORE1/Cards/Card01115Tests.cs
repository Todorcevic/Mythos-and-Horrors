using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class Card01115Tests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Resign()
        {
            CardPlace Parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.First;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Parlor)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(Parlor);
            if (!DEBUG_MODE) yield return WaitToCloneClick(1);


            yield return taskGameAction.AsCoroutine();
            Assert.That(investigator.Resign.IsActive, Is.True);
            Assert.That(investigator.InvestigatorCard.IsInPlay, Is.False);
        }

        [UnityTest]
        public IEnumerator Parley()
        {
            _reactionableControl.SubscribeAtStart(RevealMinus1Token);
            CardPlace Parlor = _cardsProvider.GetCard<Card01115>();
            Investigator investigator = _investigatorsProvider.Second;
            yield return _preparationScene.PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(Parlor, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(_preparationScene.SceneCORE1.Lita, Parlor.OwnZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, Parlor)).AsCoroutine();

            Task taskGameAction = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            if (!DEBUG_MODE) yield return WaitToClick(Parlor);
            if (!DEBUG_MODE) yield return WaitToCloneClick(2);
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();
            if (!DEBUG_MODE) yield return WaitToMainButtonClick();

            yield return taskGameAction.AsCoroutine();
            Assert.That(investigator.AidZone.Cards.Contains(_preparationScene.SceneCORE1.Lita), Is.True);
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
