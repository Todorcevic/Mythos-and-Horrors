﻿using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class StartChapterGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        public Chapter Chapter { get; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.START_CHAPTER_PHASE_NAME;
        public override string Description => _textsProvider.GameText.START_CHAPTER_PHASE_DESCRIPTION;

        /*******************************************************************/
        public StartChapterGameAction(Chapter chapter)
        {
            Chapter = chapter;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Chapter.Description));
        }
    }
}