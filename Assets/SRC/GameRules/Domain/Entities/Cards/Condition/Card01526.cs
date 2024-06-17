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
        protected override bool IsFast => false;
        private IEnumerable<CardWeapon> Firearms => ControlOwner.CurrentPlace.InvestigatorsInThisPlace.SelectMany(investigator => investigator.AidZone.Cards)
            .OfType<CardWeapon>().Where(weapon => weapon.HasThisTag(Tag.Firearm) && weapon is IBulletable);

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (!Firearms.Any()) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            IEnumerable<CardWeapon> Firearms = investigator.CurrentPlace.InvestigatorsInThisPlace.SelectMany(investigator => investigator.AidZone.Cards)
            .OfType<CardWeapon>().Where(weapon => weapon.HasThisTag(Tag.Firearm) && weapon is IBulletable);

            InteractableGameAction interactable = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Firearm");
            interactable.CreateCancelMainButton();
            foreach (CardWeapon firearm in Firearms)
            {
                interactable.CreateEffect(firearm, new Stat(0, false), Reload, PlayActionType.Choose, investigator, firearm.ControlOwner.AvatarCard);

                async Task Reload() => await _gameActionsProvider.Create(new IncrementStatGameAction(((IBulletable)firearm).AmountBullets, 3));
            }

            await _gameActionsProvider.Create(interactable);
        }
    }
}
