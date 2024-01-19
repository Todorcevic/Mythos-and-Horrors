using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        [Inject] private readonly IAnimator _animator;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; private set; }
        public Stat FromStat { get; private set; }
        public int Amount { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, int amount)
        {
            Investigator = investigator;
            FromStat = _chaptersProvider.CurrentScene.ResourcesPile;
            Amount = amount;
            await Start();
        }

        public async Task Run(Investigator investigator, Stat fromStat, int amount)
        {
            Investigator = investigator;
            FromStat = fromStat;
            Amount = fromStat.Value < amount ? fromStat.Value : amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<DecrementStatGameAction>().Run(FromStat, Amount);
            await _animator.PlayAnimationWith(this);
            await _gameActionFactory.Create<IncrementStatGameAction>().Run(Investigator.Resources, Amount);
        }
    }
}
