using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigateGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardPlace CardPlace { get; }
        public ChallengePhaseGameAction Challenge { get; private set; }

        /*******************************************************************/
        public InvestigateGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigator = investigator;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Challenge = new ChallengePhaseGameAction(
                Investigator.Intelligence,
                CardPlace.Enigma.Value,
                "Investigate" + CardPlace.Info.Name,
                succesEffect: SuccesEffet,
                cardToChallenge: CardPlace);
            await _gameActionsProvider.Create(Challenge);

            /*******************************************************************/
            async Task SuccesEffet() => await _gameActionsProvider.Create(new GainHintGameAction(Investigator, CardPlace.Hints, 1));
        }
    }
}
