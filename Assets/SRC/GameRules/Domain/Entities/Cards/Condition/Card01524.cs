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

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Card01524");
            foreach (CardPlace place in investigator.CurrentPlace.ConnectedPlacesToMove.Append(investigator.CurrentPlace))
            {
                interactable.CreateEffect(place, new Stat(0, false), Explote, PlayActionType.Choose, investigator);

                async Task Explote()
                {
                    await _gameActionsProvider.Create<SafeForeach<IDamageable>>().SetWith(AllDamageables, DoDamage).Execute();

                    /*******************************************************************/
                    IEnumerable<IDamageable> AllDamageables() => place.CreaturesInThisPlace.Cast<IDamageable>().Concat(place.InvestigatorsInThisPlace
                           .Select(investigator => investigator.InvestigatorCard).Cast<IDamageable>());

                    async Task DoDamage(IDamageable damageable)
                    {
                        if (damageable is CardInvestigator cardInvestigator)
                            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(cardInvestigator.Owner, this, amountDamage: 3).Execute();
                        else await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith((Card)damageable, this, amountDamage: 3).Execute();
                    }
                }
            }

            await interactable.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;
    }
}
