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
            Zone zone = GetZone();
            if (zone == null) await _gameActionsProvider.Create(new DiscardGameAction(CardDrawed));
            else await _gameActionsProvider.Create(new MoveCardsGameAction(CardDrawed, zone));
        }

        private Zone GetZone() => CardDrawed switch
        {
            CardAdversity cardAdversity => cardAdversity.ZoneToMove,
            ISpawnable spawnable => spawnable.SpawnPlace?.OwnZone,
            CardCreature => Investigator.DangerZone,
            _ => Investigator.HandZone
        };
    }
}
