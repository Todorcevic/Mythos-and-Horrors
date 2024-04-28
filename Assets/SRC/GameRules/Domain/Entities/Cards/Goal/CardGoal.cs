using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardGoal : Card, IRevealable, IActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProviders;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat Hints { get; private set; }
        public State Revealed { get; private set; }
        public List<Activation> Activations { get; private set; }
        public CardGoal NextCardGoal => _chaptersProviders.CurrentScene.Info.GoalCards.NextElementFor(this);
        public int MaxHints => (Info.Hints ?? 0) * _investigatorsProvider.AllInvestigators.Count;

        /*******************************************************************/
        public History InitialHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(1);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Activations = new() { new(new Stat(0), PayHintsActivate, PayHintsConditionToActivate) };
            Hints = new Stat(MaxHints);
            Revealed = new State(false);
        }

        /*******************************************************************/
        public virtual async Task RevealEffect()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
            await CompleteEffect();
            await _gameActionsProvider.Create(new DiscardGameAction(this));
            await _gameActionsProvider.Create(new PlaceGoalGameAction(NextCardGoal));
        }

        public abstract Task CompleteEffect();

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await Reaction<UpdateStatGameAction>(gameAction, RevealCondition, RevealLogic);
        }

        /*******************************************************************/
        private bool RevealCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (!updateStatGameAction.HasStat(Hints)) return false;
            if (Revealed.IsActive) return false;
            if (Hints.Value > 0) return false;
            return true;
        }

        private async Task RevealLogic(UpdateStatGameAction updateStatGameAction)
            => await _gameActionsProvider.Create(new RevealGameAction(this));

        /*******************************************************************/
        public async Task PayHintsActivate(Investigator activeInvestigator) =>
            await _gameActionsProvider.Create(new PayHintsToGoalGameAction(this, _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.Hints.Value > 0)));

        public virtual bool PayHintsConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay.Sum(investigator => investigator.Hints.Value) < Hints.Value) return false;
            return true;
        }
    }
}
