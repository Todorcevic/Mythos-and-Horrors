using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShareDamageAndFearGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public Card ByThisCard { get; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }

        public override bool CanBeExecuted => (AmountDamage > 0 || AmountFear > 0) && Investigator.IsInPlay;
        public string Description => $"Recived {AmountDamage}Damage {AmountFear}Fear";

        /*******************************************************************/
        public ShareDamageAndFearGameAction(Investigator investigator, Card bythisCard, int amountDamage = 0, int amountFear = 0)
        {
            Investigator = investigator;
            ByThisCard = bythisCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            while (CanBeExecuted)
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, Description);
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
                        .SetCardAffected(ByThisCard)
                        .SetLogic(DoDamageAndFear);

                    /*******************************************************************/
                    async Task DoDamageAndFear()
                    {
                        HarmToCardGameAction harm = await _gameActionsProvider.Create(new HarmToCardGameAction(cardSelectable, ByThisCard, AmountDamage, AmountFear));
                        AmountDamage -= harm.TotalDamageApply;
                        AmountFear -= harm.TotalFearApply;
                    }
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
        }
    }
}
