using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01179 : CardCreature, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderBy(investigator => investigator.Agility.Value).First();

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.DarkYoung };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<RoundGameAction>(HealthCondition, HealthLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task HealthLogic(RoundGameAction action)
        {
            await _gameActionsProvider.Create<HealthGameAction>().SetWith(this, amountDamageToRecovery: 2).Start();
        }

        private bool HealthCondition(RoundGameAction action)
        {
            if (!IsInPlay) return false;
            if (HealthLeft <= 0) return false;
            return true;
        }
    }
}
