using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class EludeCreatureGameAction : ChallengePhaseGameAction
    {
        public CardCreature CardCreature { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue && CardCreature.IsInPlay.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, Localization localization, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null)
            => throw new NotImplementedException();

        public EludeCreatureGameAction SetWith(Investigator investigator, CardCreature creature)
        {
            base.SetWith(investigator.Agility, creature.Agility.Value, new Localization("Challenge_EludeCreature", creature.CurrentName), cardToChallenge: creature);
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
