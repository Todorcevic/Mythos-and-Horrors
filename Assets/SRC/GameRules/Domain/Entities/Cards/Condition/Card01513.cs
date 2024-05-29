using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01513 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell, Tag.Weakness };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<PlayInvestigatorGameAction>(Condition, Logic, false);
        }

        /*******************************************************************/
        private async Task Logic(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ControlOwner, amountFear: 2, fromCard: this));
        }

        private bool Condition(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            if (CurrentZone != playInvestigatorGameAction.ActiveInvestigator.HandZone) return false;
            return true;
        }

        /*******************************************************************/

        public override async Task ExecuteConditionEffect()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 1));
            await _gameActionsProvider.Create(new CheckEldritchsPlotGameAction());
        }
    }
}
