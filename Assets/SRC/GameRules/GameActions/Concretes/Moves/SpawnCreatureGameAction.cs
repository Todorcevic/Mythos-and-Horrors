using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SpawnCreatureGameAction : GameAction
    {

        public CardCreature Creature { get; }
        public CardPlace Place { get; private set; }
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
            if (Place != null) await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Creature, Place.OwnZone).Start();
            else if (Investigator != null) await _gameActionsProvider.Create(new ConfrontCreatureGameAction(Creature, Investigator));
            else await _gameActionsProvider.Create(new DiscardGameAction(Creature));
        }

        /*******************************************************************/
        public void UpdatePlace(CardPlace cardPlace)
        {
            Place = cardPlace;
        }
    }
}
