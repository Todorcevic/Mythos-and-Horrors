using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; }
        public Stat FromStat { get; }
        public int Amount { get; }

        /*******************************************************************/
        public GainResourceGameAction(Investigator investigator, int amount)
        {
            Investigator = investigator;
            FromStat = _chaptersProvider.CurrentScene.ResourcesPile;
            Amount = amount;
        }

        public GainResourceGameAction(Investigator investigator, Stat fromStat, int amount)
        {
            Investigator = investigator;
            FromStat = fromStat;
            Amount = fromStat.Value < amount ? fromStat.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(FromStat, Amount));
            await _animator.PlayAnimationWith(this);
            await _gameActionFactory.Create(new IncrementStatGameAction(Investigator.Resources, Amount));
        }
    }
}
