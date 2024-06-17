using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01524 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override bool IsFast => false;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            InteractableGameAction interactable = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Firearm");
            interactable.CreateCancelMainButton();
            foreach (CardPlace place in investigator.CurrentPlace.ConnectedPlacesToMove.Append(investigator.CurrentPlace))
            {
                interactable.CreateCancelMainButton();
                interactable.CreateEffect(place, new Stat(0, false), Explote, PlayActionType.Choose, investigator);

                async Task Explote()
                {
                    await _gameActionsProvider.Create(new SafeForeach<IDamageable>(AllDamageables, DoDamage));

                    async Task DoDamage(IDamageable damageable)
                    {
                        if (damageable is CardInvestigator cardInvestigator)
                            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(cardInvestigator.Owner, this, amountDamage: 3));
                        else await _gameActionsProvider.Create(new HarmToCardGameAction((Card)damageable, this, amountDamage: 3));
                    }

                    IEnumerable<IDamageable> AllDamageables() => place.CreaturesInThisPlace.Cast<IDamageable>().Concat(place.InvestigatorsInThisPlace
                            .Select(investigator => investigator.InvestigatorCard).Cast<IDamageable>());
                }
            }

            await _gameActionsProvider.Create(interactable);
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;
    }
}
