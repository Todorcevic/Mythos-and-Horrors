using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public DrawDangerGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.CardDangerToDraw, Investigator.DangerZone));
            //TODO: Resolve card (Revelation, Creature, etc...)
        }
    }
}
