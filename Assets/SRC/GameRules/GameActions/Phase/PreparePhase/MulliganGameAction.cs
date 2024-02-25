using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{

    public class MulliganGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.MULLIGAN_PHASE_NAME;
        public override string Description => _textsProvider.GameText.MULLIGAN_PHASE_DESCRIPTION;

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected sealed override async Task ExecuteThisPhaseLogic()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                card.AddEffect(ActiveInvestigator, _textsProvider.GameText.MULLIGAN_EFFECT1, DiscardEffect);

                Task DiscardEffect() => _gameActionFactory.Create(new DiscardGameAction(card));
            }

            foreach (Card card in ActiveInvestigator.DiscardZone.Cards.FindAll(card => card is not IWeakness))
            {
                card.AddEffect(ActiveInvestigator, _textsProvider.GameText.MULLIGAN_EFFECT2, RestoreEffect);

                Task RestoreEffect() => _gameActionFactory.Create(new MoveCardsGameAction(card, ActiveInvestigator.HandZone));
            }

            InteractableGameAction interactableGameAction = await _gameActionFactory.Create(new InteractableGameAction(false));

            if (interactableGameAction.NothingIsSelected) return;
            await _gameActionFactory.Create(new MulliganGameAction(ActiveInvestigator));
        }
    }
}

