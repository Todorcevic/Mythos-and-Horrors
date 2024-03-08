using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Card Card { get; }

        /*******************************************************************/
        public DiscardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.TurnDown(false);
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
