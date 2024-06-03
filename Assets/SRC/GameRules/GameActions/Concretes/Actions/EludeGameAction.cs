using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class EludeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardCreature CardCreature { get; }

        /*******************************************************************/
        public EludeGameAction(Investigator investigator, CardCreature creature)
        {
            Investigator = investigator;
            CardCreature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new ChallengePhaseGameAction(
                Investigator.Agility,
                CardCreature.Agility.Value,
                "Elude " + CardCreature.Info.Name,
                succesEffect: SuccesEffet,
                cardToChallenge: CardCreature));

            /*******************************************************************/
            async Task SuccesEffet()
            {
                await _gameActionsProvider.Create(new UpdateStatesGameAction(CardCreature.Exausted, true));
                await _gameActionsProvider.Create(new MoveCardsGameAction(CardCreature, CardCreature.CurrentPlace.OwnZone));
            }
        }
    }
}
