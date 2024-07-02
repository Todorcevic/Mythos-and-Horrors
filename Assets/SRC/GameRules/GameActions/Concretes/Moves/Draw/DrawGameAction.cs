using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Card CardDrawed { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
        {
            Investigator = investigator;
            CardDrawed = cardDrawed;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            switch (CardDrawed)
            {
                case IDrawActivable cardAdversity:
                    await _gameActionsProvider.Create<PlayDrawActivableGameAction>().SetWith(cardAdversity, Investigator).Start();
                    break;
                case ISpawnable spawnable:
                    await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(spawnable).Start();
                    break;
                case CardCreature cardCreature:
                    await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(cardCreature, Investigator).Start();
                    break;
                default:
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CardDrawed, Investigator.HandZone).Start();
                    break;
            }
        }
    }
}
