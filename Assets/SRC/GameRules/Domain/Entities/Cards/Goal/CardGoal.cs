using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardGoal : Card, IEndReactionable, IRevellable
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProviders;

        public Stat Hints { get; private set; }
        public State Revealed { get; private set; }
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0, Info.Hints ?? 0);
            Revealed = new State(false);
        }

        /*******************************************************************/
        async Task IEndReactionable.WhenFinish(GameAction gameAction)
        {
            await CanShowInitialHistory(gameAction);
            await CanShowFinalHistory(gameAction);
        }

        protected virtual async Task CanShowInitialHistory(GameAction gameAction)
        {
            if (Revealed.IsActive) return;
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return;
            if (!moveCardsGameAction.Cards.Contains(this)) return;
            if (moveCardsGameAction.ToZone != _chaptersProviders.CurrentScene.GoalZone) return;
            if (InitialHistory == null) return;

            await _gameActionFactory.Create(new ShowHistoryGameAction(InitialHistory, this));
        }

        protected virtual async Task CanShowFinalHistory(GameAction gameAction)
        {
            if (Revealed.IsActive) return;
            if (gameAction is not StatGameAction statGameAction) return;
            if (statGameAction.Stat != Hints) return;
            if (Hints.Value < Info.Hints) return;
            if (InitialHistory == null) return;

            await _gameActionFactory.Create(new RevealGameAction(this));
        }
    }
}
