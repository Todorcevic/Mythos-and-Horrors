using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01149 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Woods };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ResignLogic, ResignCondition, PlayActionType.Resign, "Activation_Card01149");
        }

        /*******************************************************************/
        private async Task ResignLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<RunAwayGameAction>().SetWith(investigator).Execute();
        }

        private bool ResignCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            return true;
        }
        /*******************************************************************/
    }
}
