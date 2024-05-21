using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitCardsChallengeGameAction : InteractableGameAction, IInitializable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly IPresenter<CommitCardsChallengeGameAction> _commitPresenter;

        public Effect ButtonEffect { get; private set; }

        public ChallengePhaseGameAction CurrentChallenge { get; }
        public ChallengeType ChallengeType => ActiveInvestigator.GetChallengeType(CurrentChallenge.Stat);

        IEnumerable<ICommitable> AllCommitableCards => _investigatorsProvider.GetInvestigatorsInThisPlace(ActiveInvestigator.CurrentPlace)
             .SelectMany(investigator => investigator.HandZone.Cards)
             .OfType<ICommitable>().Where(commitableCard => commitableCard.GetChallengeValue(ChallengeType) > 0);

        /*******************************************************************/
        public CommitCardsChallengeGameAction(Investigator investigator, ChallengePhaseGameAction challenge) :
            base(canBackToThisInteractable: true, mustShowInCenter: false, "Commit cards")
        {
            ActiveInvestigator = investigator;
            CurrentChallenge = challenge;
        }

        /*******************************************************************/
        public void ExecuteSpecificInitialization()
        {
            CreateMainButton().SetLogic(Continue);

            foreach (Card commitableCard in AllCommitableCards.Cast<Card>())
            {
                Create()
                    .SetCard(commitableCard)
                    .SetInvestigator(commitableCard.ControlOwner)
                    .SetCardAffected(CurrentChallenge.CardToChallenge)
                    .SetLogic(Commit);

                async Task Commit()
                {
                    await _gameActionsProvider.Create(new CommitGameAction(commitableCard));
                    await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(ActiveInvestigator, CurrentChallenge));
                }
            }

            /*******************************************************************/
            async Task Continue() => await CurrentChallenge.ContinueChallenge();
        }

        protected override async Task ExecuteThisLogic()
        {
            await _commitPresenter.PlayAnimationWith(this);
            await base.ExecuteThisLogic();
        }

        public override async Task Undo()
        {
            await _commitPresenter.PlayAnimationWith(this);
        }
    }
}

