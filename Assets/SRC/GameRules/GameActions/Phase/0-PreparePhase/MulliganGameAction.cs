using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly EffectsProvider _effectProvider;
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
            _effectProvider.CreateMainButton()
                     .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + "Continue")
                     .SetCanPlay(() => true)
                     .SetLogic(() => Task.CompletedTask);

            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                _effectProvider.Create()
                    .SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetCanPlay(CanDiscard)
                    .SetDescription(_textsProvider.GameText.MULLIGAN_EFFECT1)
                    .SetLogic(Discard);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionFactory.Create(new DiscardGameAction(card));
                    await _gameActionFactory.Create(new MulliganGameAction(ActiveInvestigator));
                }

                bool CanDiscard()
                {
                    return true;
                }
            }

            foreach (Card card in ActiveInvestigator.DiscardZone.Cards)
            {
                _effectProvider.Create()
                    .SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetCanPlay(CanRestore)
                    .SetDescription(_textsProvider.GameText.MULLIGAN_EFFECT2)
                    .SetLogic(Restore);

                /*******************************************************************/
                async Task Restore()
                {
                    await _gameActionFactory.Create(new MoveCardsGameAction(card, ActiveInvestigator.HandZone));
                    await _gameActionFactory.Create(new MulliganGameAction(ActiveInvestigator));
                }

                bool CanRestore()
                {
                    if (card is IFlaw) return false;
                    return true;
                }
            }

            await _gameActionFactory.Create(new InteractableGameAction());
        }
    }
}

