using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

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

        private Zone GetZone()
        {
            if (CardDrawed is ISpawnable spawnable) return spawnable.SpawnPlace.OwnZone;
            if (CardDrawed is CardCreature) return Investigator.DangerZone;
            if (CardDrawed is CardAdversity) return _chaptersProvider.CurrentScene.LimboZone;

            return Investigator.HandZone;
        }
    }
}
