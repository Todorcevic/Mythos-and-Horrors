﻿using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class EludeGameAction : GameAction
    {
        public CardCreature Creature { get; private set; }
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Creature.IsInPlay.IsTrue && Investigator.IsInPlay.IsTrue;

        /*******************************************************************/
        public EludeGameAction SetWith(CardCreature creature, Investigator byThisInvestigator)
        {
            Creature = creature;
            Investigator = byThisInvestigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Creature.Exausted, true).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(Creature, Creature.CurrentPlace.OwnZone).Execute();
        }
    }
}
