using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IPresenter<ResultChallengeGameAction> _resolveChallengePresenter;

        public Stat Stat { get; init; }
        public int DifficultValue { get; init; }
        public ChallengeToken TokenRevelaed { get; init; }
        public ChallengeType ChallengeType { get; init; }
        public int ChallengeValue { get; private set; }
        public bool IsSuccessful { get; private set; }

        public IEnumerable<ICommitable> CommitsCards => _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<ICommitable>();

        /*******************************************************************/
        public ResultChallengeGameAction(Stat stat, int difficultValue, ChallengeToken revealedToken, ChallengeType challengeType)
        {
            Stat = stat;
            DifficultValue = difficultValue;
            TokenRevelaed = revealedToken;
            ChallengeType = challengeType;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeValue = Stat.Value + (TokenRevelaed.Value?.Invoke() ?? 0) + CommitsCards.Sum(commitableCard => commitableCard.GetChallengeValue(ChallengeType));
            IsSuccessful = ChallengeValue >= DifficultValue;
            await _resolveChallengePresenter.PlayAnimationWith(this);
        }
    }
}
