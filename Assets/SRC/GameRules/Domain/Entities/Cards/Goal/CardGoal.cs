using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardGoal : Card, IRevealable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public Stat Hints { get; protected set; }
        public State Revealed { get; private set; }
        public Activation<Investigator> PayHints { get; private set; }
        public GameCommand<RevealGameAction> RevealCommand { get; private set; }
        public Reaction<UpdateStatGameAction> Reveal { get; private set; }

        /*******************************************************************/
        public int Position => _chaptersProviders.CurrentScene.GoalCards.IndexOf(this);
        public CardGoal NextCardGoal => _chaptersProviders.CurrentScene.GoalCards.NextElementFor(this);
        public int MaxHints => (Info.Hints ?? 0) * _investigatorsProvider.AllInvestigators.Count();
        public int AmountOfHints => MaxHints - Hints.Value;
        public bool IsComplete => Revealed.IsActive;
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            PayHints = CreateFastActivation(PayHintsActivate, PayHintsConditionToActivate, PlayActionType.Activate);
            RevealCommand = new GameCommand<RevealGameAction>(RevealEffect);
            Hints = CreateStat(MaxHints);
            Revealed = CreateState(false);
            Reveal = CreateBaseReaction<UpdateStatGameAction>(RevealCondition, RevealLogic, GameActionTime.After);
        }

        /*******************************************************************/
        protected bool RevealCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!updateStatGameAction.HasThisStat(Hints)) return false;
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (Hints.Value > 0) return false;
            return true;
        }

        protected async Task RevealLogic(UpdateStatGameAction updateStatGameAction) =>
            await _gameActionsProvider.Create(new RevealGameAction(this));

        /*******************************************************************/
        private async Task RevealEffect(RevealGameAction revealGameAction)
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
            await CompleteEffect();
            await _gameActionsProvider.Create(new DiscardGameAction(this));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(NextCardGoal));
        }

        protected abstract Task CompleteEffect();

        /*******************************************************************/
        protected virtual async Task PayHintsActivate(Investigator activeInvestigator) =>

            await _gameActionsProvider.Create<PayHintsToGoalGameAction>()
            .SetWith(this, _investigatorsProvider.AllInvestigatorsInPlay.Where(investigator => investigator.Hints.Value > 0))
            .Start();

        protected virtual bool PayHintsConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay.Sum(investigator => investigator.Hints.Value) < Hints.Value) return false;
            return true;
        }
    }
}
