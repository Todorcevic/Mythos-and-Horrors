using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatePlaceGameAction : ChallengePhaseGameAction
    {
        public int AmountHints { get; private set; }
        public CardPlace CardPlace { get; private set; }

        /*******************************************************************/
        public InvestigatePlaceGameAction SetWith(Investigator investigator, CardPlace cardPlace)
        {
            SetWith(investigator.Intelligence, cardPlace.Enigma.Value, "Investigate " + cardPlace.Info.Name, cardToChallenge: cardPlace);
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
