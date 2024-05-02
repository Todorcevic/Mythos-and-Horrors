using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; }
        public Card Card { get; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public DrawGameAction(Investigator investigator, Card SpecificCard)
        {
            Investigator = investigator;
            Card = SpecificCard;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create(new MoveCardsGameAction(Card, GetDrawZone()));
        }

        private Zone GetDrawZone()
        {
            if (_chaptersProvider.CurrentScene.Info.DangerCards.Contains(Card)) return Investigator.DangerZone;
            return Investigator.HandZone;
        }
    }
}
