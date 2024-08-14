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

        public override string Name => _textsProvider.GetLocalizableText("PhaseName_StalkerCreaturesMove");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_StalkerCreaturesMove");
        public override Phase MainPhase => Phase.Creature;

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
