using Zenject;
using MythosAndHorrors.GameRules;
using DG.Tweening;

namespace MythosAndHorrors.GameView
{
    public class TokenMoverHandler
    {
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly StatableManager _statableManager;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public Tween GainHintsAnimation(Investigator investigator, int amount, Stat fromStat)
        {
            return DOTween.Sequence()
               .Append(_swapInvestigatorPresenter.Select(investigator))
               .Append(_areaInvestigatorViewsManager.Get(investigator).HintsTokenController
                     .Gain(amount, _statableManager.Get(fromStat).StatTransform));
        }

        public Tween PayHintsAnimation(Investigator investigator, int amount, Stat toStat)
        {
            return DOTween.Sequence()
              .Append(_swapInvestigatorPresenter.Select(investigator))
              .Append(_areaInvestigatorViewsManager.Get(investigator).HintsTokenController
                    .Pay(amount, _statableManager.Get(toStat).StatTransform));
        }

        public Tween GainResourceAnimation(Investigator investigator, int amount, Stat fromStat = null)
        {
            return DOTween.Sequence()
               .Append(_swapInvestigatorPresenter.Select(investigator))
               .Append(_areaInvestigatorViewsManager.Get(investigator).ResourcesTokenController
                   .Gain(amount, _statableManager.Get(fromStat ?? _chaptersProvider.CurrentScene.PileAmount).StatTransform));
        }

        public Tween PayResourceAnimation(Investigator investigator, int amount, Stat toStat = null)
        {
            return DOTween.Sequence()
              .Append(_swapInvestigatorPresenter.Select(investigator))
              .Append(_areaInvestigatorViewsManager.Get(investigator).ResourcesTokenController
                    .Pay(amount, _statableManager.Get(toStat ?? _chaptersProvider.CurrentScene.PileAmount).StatTransform));
        }
    }
}
