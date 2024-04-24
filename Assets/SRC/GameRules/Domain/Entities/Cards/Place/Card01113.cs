using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01113 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Reaction<MoveInvestigatorToPlaceGameAction> TakeFear { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            TakeFear = new Reaction<MoveInvestigatorToPlaceGameAction>(TakeFearCondition, TakeFearLogic);
        }

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await TakeFear.Check(gameAction);
        }

        /*******************************************************************/
        private async Task TakeFearLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await new SafeForeach<Investigator>(TekeFear, Investigators).Execute();

            IEnumerable<Investigator> Investigators() => moveInvestigatorToPlaceGameAction.Investigators;

            async Task TekeFear(Investigator investigator) =>
                await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(investigator, amountFear: 1, fromCard: this));
        }

        private bool TakeFearCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }

    }
}
