﻿using System;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RegisterChapterGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private bool _oldRegister;

        public Enum Position { get; }
        public bool State { get; }

        /*******************************************************************/
        public RegisterChapterGameAction(Enum position, bool state)
        {
            Position = position;
            State = state;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _oldRegister = _chaptersProvider.CurrentChapter.IsRegistered(Position);
            _chaptersProvider.CurrentChapter.ChapterRegister(Position, State);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            _chaptersProvider.CurrentChapter.ChapterRegister(Position, _oldRegister);
            await Task.CompletedTask;
        }
    }
}