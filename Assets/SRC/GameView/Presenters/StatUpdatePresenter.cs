using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatUpdatePresenter : IPresenter<UpdateStatGameAction>
    {
        [Inject] private readonly StatableManager _statsViewsManager;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TokenMoverHandler _tokenMoverHandler;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        async Task IPresenter<UpdateStatGameAction>.PlayAnimationWith(UpdateStatGameAction updateStatGameAction)
        {
            Dictionary<IStatable, bool> statablesUpdated = _statsViewsManager.GetAll(updateStatGameAction.AllStatsUpdated)
                .ToDictionary(statView => statView, _ => false);
            await SpecialAnimations(updateStatGameAction, statablesUpdated);

            IEnumerable<IStatable> allStatables = _statsViewsManager.GetAll(updateStatGameAction.AllStatsUpdated);
            Update(allStatables, statablesUpdated);
        }

        /*******************************************************************/
        private async Task SpecialAnimations(UpdateStatGameAction updateStatGameAction, Dictionary<IStatable, bool> statablesUpdated)
        {
            await CheckEldritch(updateStatGameAction, statablesUpdated).AsyncWaitForCompletion();
            await CheckResources(updateStatGameAction).AsyncWaitForCompletion();
            await CheckHints(updateStatGameAction, statablesUpdated).AsyncWaitForCompletion();
            await CheckHarm(updateStatGameAction, statablesUpdated).AsyncWaitForCompletion();
        }

        private Tween CheckEldritch(UpdateStatGameAction updateStatGameAction, Dictionary<IStatable, bool> statablesUpdated)
        {
            Sequence eldritchSequence = DOTween.Sequence();

            if (updateStatGameAction.HasThisStat(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch))
            {
                eldritchSequence.Append(_moveCardHandler.MoveCardtoCenter(_chaptersProvider.CurrentScene.CurrentPlot))
                    .Append(Update(_statsViewsManager.GetAll(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch), statablesUpdated))
                    .Append(_moveCardHandler.ReturnCard(_chaptersProvider.CurrentScene.CurrentPlot));
            }

            return eldritchSequence;
        }

        private Tween CheckHarm(UpdateStatGameAction updateStatGameAction, Dictionary<IStatable, bool> statablesUpdated)
        {
            Sequence harmSequence = DOTween.Sequence();
            IEnumerable<IDamageable> allDamageable = _cardsProvider.AllCards.OfType<IDamageable>()
                .Where(damagable => updateStatGameAction.HasThisStat(damagable.DamageRecived));
            allDamageable.ForEach(damagableCard => harmSequence.Join(Update(_statsViewsManager.GetAll(damagableCard.DamageRecived), statablesUpdated)));
            _cardsProvider.AllCards.OfType<IFearable>().Where(fearable => updateStatGameAction.HasThisStat(fearable.FearRecived))
             .ForEach(fearableCard => harmSequence.Join(Update(_statsViewsManager.GetAll(fearableCard.FearRecived), statablesUpdated)));
            return harmSequence;
        }

        private Tween CheckResources(UpdateStatGameAction updateStatGameAction)
        {
            Sequence payResourceSequence = DOTween.Sequence();

            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay
                        .Where(investigator => updateStatGameAction.HasThisStat(investigator.Resources)))
            {
                int amount = investigator.Resources.Value - investigator.Resources.ValueBeforeUpdate;
                Stat cardResoursable = updateStatGameAction.AllStatsUpdated.Except(new[] { investigator.Resources }).UniqueOrDefault();

                if (amount > 0)
                {
                    payResourceSequence.Append(_tokenMoverHandler.GainResourceAnimation(investigator, amount, cardResoursable));
                }
                else if (amount < 0)
                {
                    payResourceSequence.Append(_tokenMoverHandler.PayResourceAnimation(investigator, -1 * amount, cardResoursable));
                }
            }

            return payResourceSequence;
        }

        private Tween CheckHints(UpdateStatGameAction updateStatGameAction, Dictionary<IStatable, bool> statablesUpdated)
        {
            Sequence hintsSequence = DOTween.Sequence();

            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay
                       .Where(investigator => updateStatGameAction.HasThisStat(investigator.Hints)))
            {
                int amount = investigator.Hints.Value - investigator.Hints.ValueBeforeUpdate;
                Stat locationHint = updateStatGameAction.AllStatsUpdated.Except(new[] { investigator.Hints }).Unique();

                if (amount > 0)
                {
                    hintsSequence.Join(Update(_statsViewsManager.GetAll(locationHint), statablesUpdated));
                    hintsSequence.Append(_tokenMoverHandler.GainHintsAnimation(investigator, amount, locationHint));
                }
                else if (amount < 0)
                {
                    hintsSequence.Append(_tokenMoverHandler.PayHintsAnimation(investigator, -1 * amount, locationHint));
                }
            }

            return hintsSequence;
        }

        private Tween Update(IEnumerable<IStatable> statView, Dictionary<IStatable, bool> statablesUpdated)
        {
            Sequence updateSequence = DOTween.Sequence();
            foreach (IStatable stat in statView)
            {
                if (statablesUpdated[stat]) continue;
                statablesUpdated[stat] = true;
                updateSequence.Join(stat.UpdateAnimation());
            }
            return updateSequence;
        }
    }
}
