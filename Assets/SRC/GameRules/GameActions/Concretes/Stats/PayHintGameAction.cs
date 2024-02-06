using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayHintGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; }
        public Stat ToStat { get; }
        public int Amount { get; }

        /*******************************************************************/
        public PayHintGameAction(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.InvestigatorCard.Hints.Value < amount ? Investigator.InvestigatorCard.Hints.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.InvestigatorCard.Hints, Amount));
            await _viewLayersProvider.PlayAnimationWith(this);
            await _gameActionFactory.Create(new IncrementStatGameAction(ToStat, Amount));
        }
    }
}
