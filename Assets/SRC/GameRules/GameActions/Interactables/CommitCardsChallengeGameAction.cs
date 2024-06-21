using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitCardsChallengeGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly IPresenter<CommitCardsChallengeGameAction> _commitPresenter;

        public CardEffect ButtonEffect { get; private set; }
        public ChallengePhaseGameAction CurrentChallenge { get; }

        private IEnumerable<CommitableCard> AllCommitableCards => _investigatorsProvider.GetInvestigatorsInThisPlace(CurrentChallenge.ActiveInvestigator.CurrentPlace)
              .SelectMany(investigator => investigator.HandZone.Cards)
              .OfType<CommitableCard>().Where(commitableCard => commitableCard.GetChallengeValue(CurrentChallenge.ChallengeType) > 0);

        public Investigator ActiveInvestigator => CurrentChallenge.ActiveInvestigator;

        /*******************************************************************/
        public CommitCardsChallengeGameAction(ChallengePhaseGameAction challenge) :
            base(canBackToThisInteractable: true, mustShowInCenter: false, "Commit cards")
        {
            CurrentChallenge = challenge;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            CreateMainButton(CurrentChallenge.ContinueChallenge, "Drop");

            foreach (CommitableCard commitableCard in AllCommitableCards)
            {
                CreateEffect(commitableCard, new Stat(0, false), Commit, PlayActionType.Commit, commitableCard.ControlOwner, cardAffected: CurrentChallenge.CardToChallenge);

                async Task Commit()
                {
                    await _gameActionsProvider.Create(new CommitGameAction(commitableCard));
                    await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(CurrentChallenge));
                }
            }

            foreach (ChallengeActivation activation in _cardsProvider.AllCards.OfType<CardChallengeSupply>()
                .SelectMany(cardChallengeSupply => cardChallengeSupply.AllCommitsActivations)
                .Where(activation => activation.FullCondition(CurrentChallenge)))
            {
                CreateEffect(activation.Card,
                   activation.ActivateTurnsCost,
                   Activate,
                   PlayActionType.Activate | activation.PlayAction,
                   ActiveInvestigator);

                async Task Activate()
                {
                    await activation.PlayFor(CurrentChallenge);
                    await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(CurrentChallenge));
                }
            }
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _commitPresenter.PlayAnimationWith(this);
            await base.ExecuteThisLogic();
        }
    }
}

