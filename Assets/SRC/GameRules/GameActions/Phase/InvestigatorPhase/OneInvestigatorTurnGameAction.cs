using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction //2.2.1	Investigator takes an action, if able.
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            PreparePassEffect();
            CheckIfCanPlayFromHand();
            CheckIfCanActivate();
            await _gameActionFactory.Create(new InteractableGameAction());
        }

        /*******************************************************************/
        private void PreparePassEffect()
        {
            _effectProvider.CreateMainButton()
                .SetCard(null)
                .SetInvestigator(ActiveInvestigator)
                .SetCanPlay(() => true)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PassTurn))
                .SetLogic(PassTurn);

            /*******************************************************************/
            async Task PassTurn() =>
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.Turns.Value));
        }

        private void CheckIfCanActivate()
        {
            foreach (IActivable activableCard in _cardsProvider.AllCards.OfType<IActivable>())
            {
                if (activableCard.CanActivate())
                {
                    _effectProvider.Create()
                        .SetCard(activableCard as Card)
                        .SetInvestigator(ActiveInvestigator)
                        .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(activableCard.Activate))
                        .SetCanPlay(() => activableCard.CanActivate())
                        .SetLogic(ActivateCard);
                }

                /*******************************************************************/
                async Task ActivateCard()
                {
                    await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, activableCard.ActivationTurnsCost.Value));
                    await activableCard.Activate();
                }
            }
        }

        private void CheckIfCanPlayFromHand()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (card is IPlayableFromHand playableFromHand)
                {
                    _effectProvider.Create()
                        .SetCard(card)
                        .SetInvestigator(ActiveInvestigator)
                        .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PlayFromHand))
                        .SetCanPlay(() => playableFromHand.CanPlayFromHand())
                        .SetLogic(PlayFromHand);

                    /*******************************************************************/
                    async Task PlayFromHand()
                    {
                        await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, playableFromHand.TurnsCost.Value));
                        await _gameActionFactory.Create(new PayResourceGameAction(ActiveInvestigator, playableFromHand.ResourceCost.Value));
                        await playableFromHand.PlayFromHand();
                    }
                }
            }
        }
    }
}
