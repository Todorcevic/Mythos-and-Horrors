using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01513 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell, Tag.Weakness };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<PlayInvestigatorGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch, 1).Execute();
            await _gameActionsProvider.Create<CheckEldritchsPlotGameAction>().Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;

        /*******************************************************************/
        private async Task Logic(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(ControlOwner, amountFear: 2, fromCard: this).Execute();
        }

        private bool Condition(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            if (CurrentZone != playInvestigatorGameAction.ActiveInvestigator.HandZone) return false;
            return true;
        }
    }
}
