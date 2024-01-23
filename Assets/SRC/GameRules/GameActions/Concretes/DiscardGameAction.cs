using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Card Card { get; private set; }

        /*******************************************************************/
        public async Task Run(Card card)
        {
            Card = card;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card.TurnDown(false);
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(Card, GetDiscardZone());
        }

        private Zone GetDiscardZone()
        {
            if (_chaptersProvider.CurrentScene.Info.DangerCards.Contains(Card))
                return _chaptersProvider.CurrentScene.DangerDiscardZone;

            return _investigatorsProvider.GetInvestigatorWithThisCard(Card)?.DiscardZone
                ?? _chaptersProvider.CurrentScene.OutZone;
        }
    }
}
