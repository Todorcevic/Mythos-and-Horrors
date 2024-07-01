using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class EludeCreatureGameAction : ChallengePhaseGameAction
    {
        public CardCreature CardCreature { get; private set; }

        /*******************************************************************/
        public EludeCreatureGameAction SetWith(Investigator investigator, CardCreature creature)
        {
            SetWith(investigator.Agility, creature.Agility.Value, "Elude " + creature.Info.Name, cardToChallenge: creature);
            CardCreature = creature;
            SuccesEffects.Add(SuccesEffet);
            return this;
        }

        /*******************************************************************/
        private async Task SuccesEffet()
        {
            await _gameActionsProvider.Create(new EludeGameAction(CardCreature, ActiveInvestigator));
        }
    }
}
