using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //3.2	Hunter enemies move.
    public class StalkerCreaturesMoveGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(StalkerCreaturesMoveGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(StalkerCreaturesMoveGameAction);
        public override Phase MainPhase => Phase.Creature;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (IStalker creature in _cardsProvider.StalkersInPlay)
            {
                await _gameActionFactory.Create(new MoveCreatureGameAction(creature));
            }
        }
    }
}
