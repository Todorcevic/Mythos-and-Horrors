using System.Collections.Generic;
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
            RemoveStat(Keys);
            Keys = CreateStat(_investigatorsProvider.AllInvestigators.Count() * 2);
            PayKeys.ActivateActionsCost.UpdateValue(1);
            CreateForceReaction<MoveCardsGameAction>(RevealCondition, RevealLogic, GameActionTime.After);
        }

        /*******************************************************************/
        protected override async Task PayKeysActivate(Investigator activeInvestigator)
        {
            await base.PayKeysActivate(activeInvestigator);
            if (Keys.Value > 0) return;

            Card cultist = SceneCORE2.Cultists.Where(cultist => cultist.CurrentZone == SceneCORE2.OutZone).Rand();
            await _gameActionsProvider.Create<DrawGameAction>().SetWith(_investigatorsProvider.Leader, cultist).Execute();
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(Keys, _investigatorsProvider.AllInvestigators.Count() * 2).Execute();
        }

        private bool RevealCondition(MoveCardsGameAction updateStatGameAction)
        {
            if (updateStatGameAction.AllMoves.All(move => move.Value.zone != SceneCORE2.VictoryZone)) return false;
            if (IsInPlay.IsFalse) return false;
            if (Revealed.IsActive) return false;
            if (SceneCORE2.AllCultists.Any(cultist => cultist.CurrentZone != SceneCORE2.VictoryZone)) return false;
            return true;
        }

        protected async Task RevealLogic(MoveCardsGameAction updateStatGameAction) =>
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(this).Execute();

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(SceneCORE2.FullResolutions[1]).Execute();
        }
    }
}
