using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShareDamageAndFearGameAction : GameAction
    {
        private int amountDamagaRecived;
        private int amountFearRecived;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public Card FromCard { get; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }

        public override bool CanBeExecuted => (AmountDamage > 0 || AmountFear > 0) && !_isCancel;
        public string Description => $"Recived {AmountDamage}Damage {AmountFear}Fear";

        private static bool _isCancel;
        /*******************************************************************/
        public ShareDamageAndFearGameAction(Investigator investigator, int amountDamage = 0, int amountFear = 0, Card fromCard = null)
        {
            Investigator = investigator;
            FromCard = fromCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            //while (CanBeExecuted && !_isCancel)
            //{
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, Description);

            interactableGameAction.CreateUndoButton().SetLogic(Undo);
            async Task Undo()
            {
                if (amountDamagaRecived == 0 && amountFearRecived == 0) _isCancel = true;
                await _gameActionsProvider.UndoLastInteractable();
            }

            List<Card> allSelectables = new();

            if (AmountDamage > 0)
                allSelectables.AddRange(Investigator.CardsInPlay.OfType<IDamageable>().Cast<Card>().Except(allSelectables));

            if (AmountFear > 0)
                allSelectables.AddRange(Investigator.CardsInPlay.OfType<IFearable>().Cast<Card>().Except(allSelectables));

            foreach (Card cardSelectable in allSelectables)
            {
                interactableGameAction.Create()
                    .SetCard(cardSelectable)
                    .SetInvestigator(cardSelectable.Owner)
                    .SetCardAffected(FromCard)
                    .SetLogic(DoDamageAndFear);

                /*******************************************************************/
                async Task DoDamageAndFear()
                {
                    HarmToCardGameAction harm = await _gameActionsProvider.Create(new HarmToCardGameAction(cardSelectable, AmountDamage, AmountFear));
                    amountDamagaRecived = harm.TotalDamageApply;
                    amountFearRecived = harm.TotalFearApply;
                    //await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(Investigator, AmountDamage - harm.TotalDamageApply, AmountFear - harm.TotalFearApply));
                }
            }

            await _gameActionsProvider.Create(interactableGameAction);
            //}
            await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(Investigator, AmountDamage - amountDamagaRecived, AmountFear - amountFearRecived));
        }
    }
}
