using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Card Card { get; }

        /*******************************************************************/
        public DiscardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new TurnCardGameAction(Card, toFaceDown: false));
            await _gameActionFactory.Create(new MoveCardsGameAction(Card, GetDiscardZone()));
        }

        private Zone GetDiscardZone()
        {
            if (_chaptersProvider.CurrentScene.Info.DangerCards.Contains(Card))
                return _chaptersProvider.CurrentScene.DangerDiscardZone;

            return Card.Owner?.DiscardZone ?? _chaptersProvider.CurrentScene.OutZone;
        }
    }
}
