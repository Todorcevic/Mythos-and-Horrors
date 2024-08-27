using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01601 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public bool IsUniqueTarget => true;
        public Investigator TargetInvestigator => Owner;
        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Humanoid, Tag.Criminal };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley, "Activation_Card01601");
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay.IsTrue) return false;
            if (activeInvestigator.CurrentPlace != CurrentPlace) return false;
            if (activeInvestigator.Resources.Value < 4) return false;
            return true;
        }

        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<ParleyGameAction>().SetWith(PayCreature).Execute();

            /*******************************************************************/
            async Task PayCreature()
            {
                await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(activeInvestigator, 4).Execute();
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            }
        }
    }
}
