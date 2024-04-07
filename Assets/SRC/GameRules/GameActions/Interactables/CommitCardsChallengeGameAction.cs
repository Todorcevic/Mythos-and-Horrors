using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitCardsChallengeGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly IPresenter<CommitCardsChallengeGameAction> _commitPresenter;

        public Effect ButtonEffect { get; private set; }

        public override Investigator ActiveInvestigator => _investigatorsProvider.GetInvestigatorWithThisStat(_gameActionsProvider.CurrentChallenge.Stat);
        public ChallengeType ChallengeType => ActiveInvestigator.GetChallengeType(_gameActionsProvider.CurrentChallenge.Stat);

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CommitCardsChallengeGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CommitCardsChallengeGameAction);
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _commitPresenter.PlayAnimationWith(this);
            InteractableGameAction interactableGameAction = new(isUndable: false);
            ButtonEffect = interactableGameAction.CreateMainButton()
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + "Drop")
                .SetLogic(() => Task.CompletedTask);

            IEnumerable<ICommitable> allCommitableCards = _investigatorsProvider.GetInvestigatorsInThisPlace(ActiveInvestigator.CurrentPlace)
                .SelectMany(investigator => investigator.HandZone.Cards)
                .OfType<ICommitable>().Where(commitableCard => commitableCard.GetChallengeValue(ChallengeType) > 0);

            foreach (Card commitableCard in allCommitableCards.Cast<Card>())
            {
                interactableGameAction.Create()
                    .SetCard(commitableCard)
                    .SetInvestigator(commitableCard.Owner)
                    .SetInvestigatorAffected(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Commit))
                    .SetLogic(Commit);

                async Task Commit() => await _gameActionsProvider.Create(new CommitGameAction(commitableCard));
            }

            foreach (Card card in _chaptersProvider.CurrentScene.LimboZone.Cards.Where(card => card is ICommitable))
            {
                interactableGameAction.Create()
                    .SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Uncommit))
                    .SetLogic(Uncommit);

                async Task Uncommit() => await _gameActionsProvider.Create(new MoveCardsGameAction(card, card.Owner.HandZone));
            }

            await _gameActionsProvider.Create(interactableGameAction);
            if (interactableGameAction.EffectSelected == ButtonEffect) return;
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction());
        }

        public override async Task Undo()
        {
            await _commitPresenter.PlayAnimationWith(this);
        }
    }
}

