﻿using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SpawnCreatureGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardCreature Creature { get; }
        public CardPlace Place { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public SpawnCreatureGameAction(ISpawnable spawnable)
        {
            Creature = (CardCreature)spawnable;
            Place = spawnable.SpawnPlace;
        }

        public SpawnCreatureGameAction(CardCreature creature, CardPlace place)
        {
            Creature = creature;
            Place = place;
        }

        public SpawnCreatureGameAction(CardCreature creature, Investigator investigator)
        {
            Creature = creature;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Place != null) await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, Place.OwnZone));
            else if (Investigator != null) await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, Investigator.DangerZone));
            else await _gameActionsProvider.Create(new DiscardGameAction(Creature));
        }
    }
}