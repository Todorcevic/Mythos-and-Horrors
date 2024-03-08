using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainResourceGameAction : GameAction
    {
        [Inject] private readonly IPresenter<GainResourceGameAction> _gainResourcePresenter;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private readonly Stat _fromStat;

        public Investigator Investigator { get; }
        public Stat FromStat => _fromStat ?? _chaptersProvider.CurrentScene.PileAmount;
        public int Amount { get; }
        protected override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public GainResourceGameAction(Investigator investigator, int amount) // fromStat ResourcePile -> See line 14
        {
            Investigator = investigator;
            Amount = amount;
        }

        public GainResourceGameAction(Investigator investigator, Stat fromStat, int amount)
        {
            Investigator = investigator;
            _fromStat = fromStat;
            Amount = fromStat.Value < amount ? fromStat.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(FromStat, Amount));
            await _gainResourcePresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new IncrementStatGameAction(Investigator.Resources, Amount));
        }
    }
}
