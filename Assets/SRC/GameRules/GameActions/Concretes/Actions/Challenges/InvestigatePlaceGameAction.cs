using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatePlaceGameAction : ChallengePhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public int AmountHints { get; private set; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public InvestigatePlaceGameAction(Investigator investigator, CardPlace cardPlace, int intelligenceModifier = 0)
            : base(investigator.Intelligence, cardPlace.Enigma.Value, "Investigate " + cardPlace.Info.Name, cardToChallenge: cardPlace, statModifier: intelligenceModifier)
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
