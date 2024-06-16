using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01181 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        Investigator ITarget.TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderBy(investigator => investigator.Strength.Value).First();

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Humanoid, Tag.DeepOne };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<ConfrontCreatureGameAction>(TakeFearCondition, TakeFearLogic, isAtStart: false);
        }

        /*******************************************************************/
        private async Task TakeFearLogic(ConfrontCreatureGameAction confrontCreatureGameAction)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ConfrontedInvestigator, this, amountFear: 1));
        }

        private bool TakeFearCondition(ConfrontCreatureGameAction confrontCreatureGameAction)
        {
            if (confrontCreatureGameAction.Creature != this) return false;
            return true;
        }

        /*******************************************************************/
    }

}
