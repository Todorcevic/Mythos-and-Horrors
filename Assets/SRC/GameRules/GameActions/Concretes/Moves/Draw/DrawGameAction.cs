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
            await _gameActionsProvider.Create(new MoveCardsGameAction(CardDrawed, GetZone()));
        }

        private Zone GetZone() => CardDrawed switch
        {
            CardAdversity cardAdversity => cardAdversity.ZoneToMove,
            ISpawnable spawnable => spawnable.SpawnPlace.OwnZone,
            CardCreature => Investigator.DangerZone,
            _ => Investigator.HandZone
        };
    }
}
