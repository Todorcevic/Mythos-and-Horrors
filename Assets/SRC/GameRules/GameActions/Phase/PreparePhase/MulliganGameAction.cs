using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{

    public class MulliganGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public Investigator Investigator { get; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.MULLIGAN_PHASE_NAME;
        public override string Description => _textsProvider.GameText.MULLIGAN_PHASE_DESCRIPTION;

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected sealed override async Task ExecuteThisPhaseLogic()
        {
            foreach (Card card in Investigator.HandZone.Cards)
            {
                card.AddEffect(Investigator, _textsProvider.GameText.MULLIGAN_EFFECT1, DiscardEffect);

                Task DiscardEffect() => _gameActionFactory.Create(new DiscardGameAction(card));
            }

            foreach (Card card in Investigator.DiscardZone.Cards.FindAll(card => card is not IWeakness))
            {
                card.AddEffect(Investigator, _textsProvider.GameText.MULLIGAN_EFFECT2, RestoreEffect);

                Task RestoreEffect() => _gameActionFactory.Create(new MoveCardsGameAction(card, Investigator.HandZone));
            }

            InteractableGameAction interactableGameAction = await _gameActionFactory.Create(new InteractableGameAction(false));

            if (interactableGameAction.NothingIsSelected) return;
            await _gameActionFactory.Create(new MulliganGameAction(Investigator));
        }
    }
}

