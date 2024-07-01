using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatePlaceGameAction : ChallengePhaseGameAction
    {
        public int AmountHints { get; private set; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public InvestigatePlaceGameAction(Investigator investigator, CardPlace cardPlace)
            : base(investigator.Intelligence, cardPlace.Enigma.Value, "Investigate " + cardPlace.Info.Name, cardToChallenge: cardPlace)
        {
            AmountHints = 1;
            CardPlace = cardPlace;
            SuccesEffects.Add(SuccesEffet);
        }

        /*******************************************************************/
        private async Task SuccesEffet() => await _gameActionsProvider.Create(new GainHintGameAction(ActiveInvestigator, CardPlace.Hints, AmountHints));

        public void UpdateAmountHints(int newAmountHints) => AmountHints = newAmountHints;
    }
}
