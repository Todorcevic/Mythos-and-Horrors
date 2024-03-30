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
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly IPresenter<CommitCardsChallengeGameAction> _commitPresenter;

        public Stat Stat { get; init; }
        public int Vs { get; init; }
        public ChallengeType ChallengeType { get; init; }
        public Effect ButtonEffect { get; private set; }

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CommitCardsChallengeGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CommitCardsChallengeGameAction);
        public override Phase MainPhase => Phase.Challenge;

        /*******************************************************************/
        public CommitCardsChallengeGameAction(Stat stat, int vs)
        {
            Stat = stat;
            Vs = vs;
            ActiveInvestigator = _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
            ChallengeType = ActiveInvestigator.GetChallengeType(Stat);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _commitPresenter.PlayAnimationWith(this); //Show and Update UI

            InteractableGameAction interactableGameAction = new();
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

            await _gameActionsProvider.Create(interactableGameAction);
            if (interactableGameAction.EffectSelected == ButtonEffect) return;
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(Stat, Vs));
        }
    }
}

