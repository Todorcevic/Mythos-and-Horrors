using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class EludeCreatureGameAction : ChallengePhaseGameAction
    {
        public CardCreature CardCreature { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay && CardCreature.IsInPlay;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, string localizableCode, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null, params string[] localizableArgs)
            => throw new NotImplementedException();

        public EludeCreatureGameAction SetWith(Investigator investigator, CardCreature creature)
        {
            base.SetWith(investigator.Agility, creature.Agility.Value, "Challenge_EludeCreature", cardToChallenge: creature, localizableArgs: creature.Info.Name);
            CardCreature = creature;
            SuccesEffects.Add(SuccesEffet);
            return this;
        }

        /*******************************************************************/
        private async Task SuccesEffet()
        {
            await _gameActionsProvider.Create<EludeGameAction>().SetWith(CardCreature, ActiveInvestigator).Execute();
        }
    }
}
