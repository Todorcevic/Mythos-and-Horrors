﻿using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class AddRequerimentCardGameAction : GameAction
    {
        public Investigator Investigator { get; }
        public Card Card { get; }

        /*******************************************************************/
        public AddRequerimentCardGameAction(Investigator investigator, Card card)
        {
            Investigator = investigator;
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator.Cards.Add(Card);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            Investigator.Cards.Remove(Card);
            await base.Undo();
        }
    }
}
