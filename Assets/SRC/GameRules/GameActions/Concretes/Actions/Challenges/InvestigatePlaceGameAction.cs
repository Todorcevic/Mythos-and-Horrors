using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatePlaceGameAction : ChallengePhaseGameAction
    {
        public int AmountHints { get; private set; }
        public CardPlace CardPlace { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay && CardPlace.CanBeInvestigated.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, string localizableCode, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null, params string[] localizableArgs)
            => throw new NotImplementedException();

        public InvestigatePlaceGameAction SetWith(Investigator investigator, CardPlace cardPlace)
        {
            base.SetWith(investigator.Intelligence, cardPlace.Enigma.Value, "Challenge_InvestigatePlace", cardToChallenge: cardPlace, localizableArgs: cardPlace.Info.Name);
            AmountHints = 1;
            CardPlace = cardPlace;
            SuccesEffects.Add(SuccesEffet);
            return this;
        }

        /*******************************************************************/
        private async Task SuccesEffet() => await _gameActionsProvider.Create<GainHintGameAction>().SetWith(ActiveInvestigator, CardPlace.Hints, AmountHints).Execute();

        public void UpdateAmountHints(int newAmountHints) => AmountHints = newAmountHints;
    }
}
