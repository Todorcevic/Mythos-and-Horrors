using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01114 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Reaction<MoveInvestigatorToPlaceGameAction> TakeDamage { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            TakeDamage = new Reaction<MoveInvestigatorToPlaceGameAction>(TakeDamageCondition, TakeDamageLogic);
        }

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await TakeDamage.CheckToReact(gameAction);
        }

        /*******************************************************************/
        private async Task TakeDamageLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(moveInvestigatorToPlaceGameAction.Investigators, TekeDamage));

            async Task TekeDamage(Investigator investigator) =>
                await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(investigator, amountDamage: 1, bythisCard: this));
        }

        private bool TakeDamageCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
