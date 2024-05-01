using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01601 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public bool IsOnlyOneTarget => true;
        public Investigator TargetInvestigator => Owner;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ParleyActivate, ParleyConditionToActivate);
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (activeInvestigator.CurrentPlace != CurrentPlace) return false;
            if (activeInvestigator.Resources.Value < 4) return false;
            return true;
        }

        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new ParleyGameAction(PayCreature));

            /*******************************************************************/
            async Task PayCreature()
            {
                await _gameActionsProvider.Create(new PayResourceGameAction(activeInvestigator, 4));
                await _gameActionsProvider.Create(new DiscardGameAction(this));
            }
        }
    }
}
