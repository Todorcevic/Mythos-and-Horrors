using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatePlaceGameAction : ChallengePhaseGameAction
    {
        public int AmountKeys { get; private set; }
        public CardPlace CardPlace { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue && CardPlace.CanBeInvestigated.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, Localization localization, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null)
            => throw new NotImplementedException();

        public InvestigatePlaceGameAction SetWith(Investigator investigator, CardPlace cardPlace)
        {
            base.SetWith(investigator.Intelligence, cardPlace.Enigma.Value, new Localization("Challenge_InvestigatePlace", cardPlace.CurrentName), cardToChallenge: cardPlace);
            AmountKeys = 1;
            CardPlace = cardPlace;
            SuccesEffects.Add(SuccesEffet);
            return this;
        }

        /*******************************************************************/
        private async Task SuccesEffet() => await _gameActionsProvider.Create<GainKeyGameAction>().SetWith(ActiveInvestigator, CardPlace.Keys, AmountKeys).Execute();

        public void UpdateAmountKeys(int newAmountKeys) => AmountKeys = newAmountKeys;
    }
}
