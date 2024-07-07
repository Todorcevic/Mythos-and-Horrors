using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; private set; }
        public Card CardDrawed { get; protected set; }
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
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CardDrawed, Investigator.HandZone).Execute();
                    break;
            }
        }
    }
}
