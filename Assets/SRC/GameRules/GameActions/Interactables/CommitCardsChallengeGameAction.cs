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

        public Stat Stat { get; init; }
        public int Vs { get; init; }
        public ChallengeType ChallengeType { get; init; }

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
            InteractableGameAction interactableGameAction = new();

            IEnumerable<ICommitable> allCommitableCards = _investigatorsProvider.AllInvestigatorsInPlay.SelectMany(investigator => investigator.HandZone.Cards)
                .OfType<ICommitable>().Where(commitableCard => commitableCard.GetChallengeValue(ChallengeType.Strength) > 0);

            foreach (ICommitable commitableCard in allCommitableCards)
            {
                Card card = (Card)commitableCard;

                interactableGameAction.Create()
                    .SetCard(card)
                    .SetInvestigator(card.Owner)
                    .SetInvestigatorAffected(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Commit))
                    .SetLogic(Commit);

                async Task Commit() => await _gameActionsProvider.Create(new CommitGameAction(card));
            }



            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(Stat, Vs));
        }
    }
}

