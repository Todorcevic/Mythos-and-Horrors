﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class HideCardsGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IEnumerable<Card> Cards { get; }
        public override bool CanUndo => false;
        public override bool CanBeExecuted => Cards.Any(card => !card.FaceDown.IsActive);

        /*******************************************************************/
        public HideCardsGameAction(Card card) : this(new List<Card> { card }) { }

        public HideCardsGameAction(IEnumerable<Card> cards)
        {
            Cards = cards;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Cards.Select(card => card.FaceDown), true));
        }
    }
}