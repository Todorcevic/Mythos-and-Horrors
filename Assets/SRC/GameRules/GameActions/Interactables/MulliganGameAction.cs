using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MulliganGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        List<Effect> DiscardEffects { get; } = new();
        List<Effect> RestoreEffects { get; } = new();

        /*******************************************************************/
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
                     .SetLogic(() => Task.CompletedTask);

            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                DiscardEffects.Add(_effectProvider.Create()
                    .SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.MULLIGAN_EFFECT1)
                    .SetLogic(Discard));

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new MulliganGameAction(ActiveInvestigator));
                }
            }

            foreach (Card card in ActiveInvestigator.DiscardZone.Cards)
            {
                if (!CanRestore()) continue;

                RestoreEffects.Add(_effectProvider.Create()
                    .SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.MULLIGAN_EFFECT2)
                    .SetLogic(Restore));

                /*******************************************************************/
                async Task Restore()
                {
                    await _gameActionsProvider.Create(new MoveCardsGameAction(card, ActiveInvestigator.HandZone));
                    await _gameActionsProvider.Create(new MulliganGameAction(ActiveInvestigator));
                }

                bool CanRestore()
                {
                    if (card is IFlaw) return false;
                    return true;
                }
            }

            await _gameActionsProvider.Create(new InteractableGameAction());
        }
    }
}

