﻿using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureAttackGameAction : GameAction
    {
        [Inject] private readonly IPresenter<CreatureAttackGameAction> _creatureAttackPresenter;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public CardCreature Creature { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public CreatureAttackGameAction(CardCreature creature, Investigator investigator)
        {
            Creature = creature;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _creatureAttackPresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.Health, Creature.Damage.Value));
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.Sanity, Creature.Fear.Value));
        }
    }
}