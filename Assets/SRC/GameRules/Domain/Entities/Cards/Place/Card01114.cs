using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01114 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(TakeDamageCondition, TakeDamageLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task TakeDamageLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsHere, TekeDamage).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsHere() => moveInvestigatorToPlaceGameAction.Investigators.Where(investigator => investigator.CurrentPlace == this);
            async Task TekeDamage(Investigator investigator) =>
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, amountDamage: 1, fromCard: this).Execute();
        }

        private bool TakeDamageCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
