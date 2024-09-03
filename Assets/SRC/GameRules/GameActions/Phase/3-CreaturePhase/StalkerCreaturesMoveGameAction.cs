using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //3.2	Hunter enemies move.
    public class StalkerCreaturesMoveGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override Phase MainPhase => Phase.Creature;
        public override Localization PhaseNameLocalization => new("PhaseName_StalkerCreaturesMove");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_StalkerCreaturesMove");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<IStalker>>().SetWith(StalkersInPlay, Move).Execute();
        }

        /*******************************************************************/
        private IEnumerable<IStalker> StalkersInPlay() => _cardsProvider.AllCards.OfType<IStalker>().Where(stalker => stalker.CurrentPlace != null);

        private async Task Move(IStalker stalker) => await _gameActionsProvider.Create<MoveCreatureGameAction>().SetWith(stalker, _investigatorsProvider.AllInvestigatorsInPlay).Execute();
    }
}
