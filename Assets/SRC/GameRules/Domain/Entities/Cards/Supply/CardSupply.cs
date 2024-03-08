using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardSupply : Card, IStartReactionable
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public Stat ResourceCost { get; private set; }
        public Stat TurnsCost { get; private set; }
        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }
        public int TotalChallengePoints => (Info.Strength ?? 0) + (Info.Agility ?? 0) + (Info.Intelligence ?? 0) + (Info.Power ?? 0) + (Info.Wild ?? 0);

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            ResourceCost = new Stat(Info.Cost ?? 0);
            TurnsCost = new Stat(1);
            Health = new Stat(Info.Health ?? 0, Info.Health ?? 0);
            Sanity = new Stat(Info.Sanity ?? 0, Info.Sanity ?? 0);
        }
        /*******************************************************************/
        public virtual async Task WhenBegin(GameAction gameAction)
        {
            CheckPlayFromHand(gameAction);
            await Task.CompletedTask;
        }

        /*************************** PLAY FROM HAND *********************************/
        protected void CheckPlayFromHand(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneTurnGA) return;

            _effectProvider.Create()
                .SetCard(this)
                .SetInvestigator(_investigatorProvider.ActiveInvestigator)
                .SetCanPlay(CanPlayFromHand)
                .SetLogic(PlayFromHand)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PlayFromHand));
        }

        protected bool CanPlayFromHand()
        {
            if (_investigatorProvider.ActiveInvestigator.HandZone != CurrentZone) return false;
            if (_investigatorProvider.ActiveInvestigator.Resources.Value < ResourceCost.Value) return false;
            if (_investigatorProvider.ActiveInvestigator.Turns.Value < TurnsCost.Value) return false;
            return true;
        }

        protected async Task PlayFromHand()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(_investigatorProvider.ActiveInvestigator.Turns, TurnsCost.Value));
            await _gameActionFactory.Create(new PayResourceGameAction(_investigatorProvider.ActiveInvestigator, ResourceCost.Value));
            await _gameActionFactory.Create(new DiscardGameAction(this));
        }
    }
}
