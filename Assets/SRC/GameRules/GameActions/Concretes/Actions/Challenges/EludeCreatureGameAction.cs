using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EludeCreatureGameAction : ChallengePhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardCreature CardCreature { get; }

        /*******************************************************************/
        public EludeCreatureGameAction(Investigator investigator, CardCreature creature, int agilityModifier = 0)
            : base(investigator.Agility, creature.Agility.Value, "Elude " + creature.Info.Name, cardToChallenge: creature, statModifier: agilityModifier)
        {
            CardCreature = creature;
            SuccesEffects.Add(SuccesEffet);
        }

        /*******************************************************************/
        private async Task SuccesEffet()
        {
            await _gameActionsProvider.Create(new EludeGameAction(CardCreature));
        }
    }
}
