using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01511 : CardAdversity, IVictoriable, IResetable
    {
        private const int AMOUNT_RESOURCE_NEEDED = 6;

        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat AbilityUsed { get; private set; }
        public Stat Resources { get; private set; }
        public IEnumerable<Investigator> InvestigatorsVictoryAffected => new[] { Owner };

        int IVictoriable.Victory => -2;
        bool IVictoriable.IsVictoryComplete => IsInPlay && Resources.Value > 0;
        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Task };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            AbilityUsed = CreateStat(0);
            Resources = ExtraStat = CreateStat(AMOUNT_RESOURCE_NEEDED);
            CreateActivation(CreateStat(0), PayResourceActivate, PayResourceConditionToActivate, PlayActionType.Activate);
            CreateReaction<RoundGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayAdversityFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private async Task RestartAbilityLogic(RoundGameAction roudnGameAction)
        {
            await _gameActionsProvider.Create(new UpdateStatGameAction(AbilityUsed, 0));
        }

        private bool RestartAbilityCondition(RoundGameAction roudnGameAction)
        {
            if (AbilityUsed.Value < 2) return false;
            return true;
        }

        /*******************************************************************/
        private bool PayResourceConditionToActivate(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Danger) return false;
            if ((investigator.CurrentPlace != ControlOwner.CurrentPlace)) return false;
            if (AbilityUsed.Value > 1) return false;
            if (investigator.Resources.Value < 1) return false;
            if (Resources.Value < 1) return false;
            return true;
        }

        private async Task PayResourceActivate(Investigator investigator)
        {
            Dictionary<Stat, int> resources = new()
            {
                { investigator.Resources, 1 },
                { Resources, 1 }
            };
            await _gameActionsProvider.Create(new DecrementStatGameAction(resources));
            await _gameActionsProvider.Create(new IncrementStatGameAction(AbilityUsed, 1));
        }

        public async Task Reset()
        {
            await _gameActionsProvider.Create(new UpdateStatGameAction(Resources, Resources.InitialValue));
        }
    }
}
