using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardGoal : Card, IRevealable
    {
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public Stat Hints { get; private set; }
        public State Revealed { get; private set; }
        public int Position => _chaptersProviders.CurrentScene.Info.GoalCards.IndexOf(this);
        public CardGoal NextCardGoal => _chaptersProviders.CurrentScene.Info.GoalCards.ElementAtOrDefault(Position + 1);
        public bool IsComplete => Hints.Value <= 0;

        /*******************************************************************/
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0);
            Revealed = new State(false);
        }

        /*******************************************************************/
        public virtual async Task RevealEffect()
        {
            await _gameActionProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
            await _gameActionProvider.Create(new DiscardGameAction(this));
            await _gameActionProvider.Create(new PlaceGoalGameAction(NextCardGoal));
        }
    }
}
