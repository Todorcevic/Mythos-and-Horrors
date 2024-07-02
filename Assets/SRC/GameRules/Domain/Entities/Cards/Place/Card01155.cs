using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01155 : CardPlace
    {
        [Inject] private GameActionsProvider _gameActionsProvider;

        public State Used { get; private set; }

        public override IEnumerable<Tag> Tags => new[] { Tag.Woods };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Used = CreateState(false);
            CreateActivation(1, HealthDamageLogic, HealthCondition, PlayActionType.Activate);
            CreateActivation(1, HealthFearLogic, HealthCondition, PlayActionType.Activate);
            CreateForceReaction<PlayInvestigatorGameAction>(ResetLogic, ResetCondition, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task ResetCondition(PlayInvestigatorGameAction playInvestigatorGameAction) =>
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Used, false).Start();

        private bool ResetLogic(PlayInvestigatorGameAction playInvestigatorGameAction) => true;

        /*******************************************************************/
        private async Task HealthFearLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<HealthGameAction>().SetWith(investigator.InvestigatorCard, amountFearToRecovery: 1).Start();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Used, true).Start();
        }

        private async Task HealthDamageLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<HealthGameAction>().SetWith(investigator.InvestigatorCard, amountDamageToRecovery: 1).Start();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Used, true).Start();
        }

        private bool HealthCondition(Investigator investigator)
        {
            if (Used.IsActive) return false;
            if (investigator.CurrentPlace != this) return false;
            return true;
        }

        /*******************************************************************/
    }
}
