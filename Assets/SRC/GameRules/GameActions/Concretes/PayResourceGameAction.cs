using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        [Inject] private readonly IResourceAnimator _resourceAnimator;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; private set; }
        public Stat ToStat { get; private set; }
        public int Amount { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.Resources.Value < amount ? Investigator.Resources.Value : amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<UpdateStatGameAction>().Run(Investigator.Resources, Investigator.Resources.Value - Amount);
            await _resourceAnimator.PayResource(Investigator, Amount, ToStat);
            await _gameActionFactory.Create<UpdateStatGameAction>().Run(ToStat, ToStat.Value + Amount);
        }
    }
}
