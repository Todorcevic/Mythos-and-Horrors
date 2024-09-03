using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReadyAllCardsGameAction : PhaseGameAction
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        public override Phase MainPhase => Phase.Restore;
        public override Localization PhaseNameLocalization => new("PhaseName_ReadyAllCards");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_ReadyAllCards");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            IEnumerable<State> exhaustedStates = _cardsProvider.GetCardsExhausted().Select(card => card.Exausted);
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(exhaustedStates, false).Execute();
        }
    }
}
