using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

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

            Task dadsa = _gameActionsProvider.Create(new MoveCardsGameAction(cardToMove, _chaptersProvider.CurrentScene.DangerDeckZone));

            dadsa.Wait();

            Assert.That(_chaptersProvider.CurrentScene.DangerDeckZone.Cards.Contains(cardToMove), Is.False);
        }
    }
}
