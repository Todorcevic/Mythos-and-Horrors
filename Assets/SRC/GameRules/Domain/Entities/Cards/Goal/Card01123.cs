using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01123 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProviders.CurrentScene;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Reveal.Disable();
            PayHints.Disable();
            Hints = CreateStat(_investigatorsProvider.AllInvestigators.Count() * 2);
            CreateActivation(CreateStat(1), PayHintsActivate, PayHintsConditionToActivate, isBase: true);
            CreateReaction<MoveCardsGameAction>(RevealCondition, RevealLogic, isAtStart: false);
            CreateReaction<PayHintsToGoalGameAction>(DrawCultistCondition, DrawCultistLogic, isAtStart: false);
        }

        /*******************************************************************/
        private async Task DrawCultistLogic(PayHintsToGoalGameAction payHintGameActionn)
        {
            Card cultist = SceneCORE2.Cultists.Where(cultist => cultist.CurrentZone == SceneCORE2.OutZone).Rand();
            await _gameActionsProvider.Create(new DrawGameAction(_investigatorsProvider.Leader, cultist));
            await _gameActionsProvider.Create(new UpdateStatGameAction(Hints, _investigatorsProvider.AllInvestigators.Count() * 2));
        }

        private bool DrawCultistCondition(PayHintsToGoalGameAction payHintGameActionn)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (Hints.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
        private bool RevealCondition(MoveCardsGameAction updateStatGameAction)
        {
            if (updateStatGameAction.AllMoves.All(move => move.Value.zone != SceneCORE2.VictoryZone)) return false;
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (SceneCORE2.AllCultists.Any(cultist => cultist.CurrentZone != SceneCORE2.VictoryZone)) return false;
            return true;
        }

        protected async Task RevealLogic(MoveCardsGameAction updateStatGameAction) =>
            await _gameActionsProvider.Create(new RevealGameAction(this));

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(SceneCORE2.FullResolutions[1]));

        }
    }
}
