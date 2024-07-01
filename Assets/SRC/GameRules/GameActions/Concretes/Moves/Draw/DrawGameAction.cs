using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {

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
                case IDrawActivable cardAdversity:
                    await _gameActionsProvider.Create(new PlayDrawActivableGameAction(cardAdversity, Investigator));
                    break;
                case ISpawnable spawnable:
                    await _gameActionsProvider.Create(new SpawnCreatureGameAction(spawnable));
                    break;
                case CardCreature cardCreature:
                    await _gameActionsProvider.Create(new SpawnCreatureGameAction(cardCreature, Investigator));
                    break;
                default:
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CardDrawed, Investigator.HandZone).Start();
                    break;
            }
        }
    }
}
