using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayersProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; }
        public Stat ToStat { get; }
        public int Amount { get; }

        /*******************************************************************/
        public PayResourceGameAction(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.InvestigatorCard.Resources.Value < amount ? Investigator.InvestigatorCard.Resources.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.InvestigatorCard.Resources, Amount));
            await _viewLayersProvider.PlayAnimationWith(this);
            await _gameActionFactory.Create(new IncrementStatGameAction(ToStat, Amount));
        }
    }
}
