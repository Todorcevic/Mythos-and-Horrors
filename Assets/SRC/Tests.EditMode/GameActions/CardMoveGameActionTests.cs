using MythosAndHorrors.GameRules;
using NUnit.Framework;

namespace MythosAndHorrors.EditMode.Tests
{

    [TestFixture]
    public class CardMoveGameActionTests : SetupAutoInject
    {
        [Test]
        public void SimpleMove()
        {
            _prepareGameRulesUseCase.Execute();

            Card cardToMove = _cardsProvider.GetCardByCode("01105");

            _gameActionsProvider.Create(new MoveCardsGameAction(cardToMove, _chaptersProvider.CurrentScene.DangerDeckZone)).Wait();

            Assert.That(_chaptersProvider.CurrentScene.DangerDeckZone.Cards.Contains(cardToMove), Is.True);
        }
    }
}
