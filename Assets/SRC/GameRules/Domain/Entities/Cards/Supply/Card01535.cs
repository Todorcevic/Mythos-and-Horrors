using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01535 : CardSupply, IActivable
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat ActivationTurnsCost { get; private set; }
        public Effect ActivateEffect { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            ActivationTurnsCost = new Stat(1);
        }

        /*******************************************************************/
        bool IActivable.CanActivate()
        {
            if (!Owner.AidZone.Cards.Contains(this)) return false;
            if (Owner.Turns.Value < ActivationTurnsCost.Value) return false;
            if (_investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace).Count < 1) return false;
            return true;
        }

        async Task IActivable.Activate()
        {
            //ChooseInvestigatorGameAction chooseInvestigatorGA =
            //    await _gameActionFactory.Create(new ChooseInvestigatorGameAction(_investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace)));
            await _gameActionFactory.Create(new IncrementStatGameAction(Owner.Health, 1));
        }
    }
}
