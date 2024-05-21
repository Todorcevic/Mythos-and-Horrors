using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class DrawGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public Card CardDrawed { get; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public DrawGameAction(Investigator investigator, Card cardDrawed)
        {
            Investigator = investigator;
            CardDrawed = cardDrawed;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            switch (CardDrawed)
            {
                case CardAdversity cardAdversity:
                    await _gameActionsProvider.Create(new MoveCardsGameAction(cardAdversity, cardAdversity.ZoneToMove));
                    break;
                case ISpawnable spawnable:
                    await _gameActionsProvider.Create(new SpawnCreatureGameAction(spawnable));
                    break;
                case CardCreature cardCreature:
                    await _gameActionsProvider.Create(new SpawnCreatureGameAction(cardCreature, Investigator));
                    break;
                default:
                    await _gameActionsProvider.Create(new MoveCardsGameAction(CardDrawed, Investigator.HandZone));
                    break;
            }
        }
    }
}
