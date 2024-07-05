using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public IEnumerable<Card> CardsDrawed { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public DrawGameAction SetWith(Investigator investigator, Card cardDrawed) => SetWith(investigator, new[] { cardDrawed });

        public DrawGameAction SetWith(Investigator investigator, IEnumerable<Card> cardsDrawed)
        {
            Investigator = investigator;
            CardsDrawed = cardsDrawed;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Card>>().SetWith(CardsToDraw, Draw).Execute();
        }

        private IEnumerable<Card> CardsToDraw() => CardsDrawed;

        private async Task Draw(Card card)
        {
            switch (card)
            {
                case IDrawRevelation cardAdversity:
                    await _gameActionsProvider.Create<PlayDrawRevelationGameAction>().SetWith(cardAdversity, Investigator).Execute();
                    break;
                case ISpawnable spawnable:
                    await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(spawnable).Execute();
                    break;
                case CardCreature cardCreature:
                    await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(cardCreature, Investigator).Execute();
                    break;
                default:
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(card, Investigator.HandZone).Execute();
                    break;
            }
        }
    }
}
