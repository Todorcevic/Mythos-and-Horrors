using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01147 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private SceneCORE3 SceneCORE3 => (SceneCORE3)_chapterProvider.CurrentScene;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Reveal.Disable();
            PayHints.Disable();
            CreateForceReaction<RevealGameAction>(RevealRituaReactionCondition, RevealRituaReactionlLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task RevealRituaReactionlLogic(RevealGameAction revealGameAction)
        {
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(this).Execute();
        }

        private bool RevealRituaReactionCondition(RevealGameAction revealGameAction)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (revealGameAction.RevellableCard != SceneCORE3.Ritual) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(SceneCORE3.DangerDiscardZone.Cards, SceneCORE3.DangerDeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(SceneCORE3.DangerDeckZone).Execute();
            await SpawnCreature();
            if (_investigatorsProvider.AllInvestigators.Count() > 2) await SpawnCreature();
        }

        private async Task SpawnCreature()
        {
            while (SceneCORE3.DangerDeckZone.TopCard is not CardCreature)
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(SceneCORE3.DangerDeckZone.TopCard).Execute();

            CardCreature creature = (CardCreature)SceneCORE3.DangerDeckZone.TopCard;
            await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(creature, SceneCORE3.Ritual).Execute();
        }
    }
}
