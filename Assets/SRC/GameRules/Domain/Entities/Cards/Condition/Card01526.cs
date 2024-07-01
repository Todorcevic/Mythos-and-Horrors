using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01526 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Supply };
        private IEnumerable<CardWeapon> Firearms(Investigator investigator) => investigator.CurrentPlace.InvestigatorsInThisPlace.SelectMany(investigator => investigator.AidZone.Cards)
            .OfType<CardWeapon>().Where(weapon => weapon.HasThisTag(Tag.Firearm) && weapon is IChargeable chargeable && chargeable.Charge.ChargeType == ChargeType.Bullet);

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!Firearms(ControlOwner).Any()) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Firearm");
            foreach (CardWeapon firearm in Firearms(investigator))
            {
                interactable.CreateEffect(firearm, new Stat(0, false), Reload, PlayActionType.Choose, investigator, firearm.ControlOwner.AvatarCard);

                async Task Reload() => await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(((IChargeable)firearm).Charge.Amount, 3).Start();
            }

            await interactable.Start();
        }
    }
}
