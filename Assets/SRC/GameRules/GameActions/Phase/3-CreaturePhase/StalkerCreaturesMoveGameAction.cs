using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //3.2	Hunter enemies move.
    public class StalkerCreaturesMoveGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
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
            await new SafeForeach<IStalker>(Move, GetStalkers).Execute();
        }

        /*******************************************************************/
        private IEnumerable<IStalker> GetStalkers() => _cardsProvider.StalkersInPlay;

        private async Task Move(IStalker stalker) =>
            await _gameActionsProvider.Create(new MoveCreatureGameAction(stalker));
    }
}
