using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01528 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Police };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);
            CreateBuff(CardsToBuff, GainStrenghtActivationLogic, GainStrenghtDeactivationLogic, new Localization("Buff_Card01528"));
            CreateFastActivation(Logic, Condition, PlayActionType.Activate, new Localization("Activation_Card01528"));
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Exausted.IsActive) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01528"));
            foreach (CardCreature cardCreature in investigator.CreaturesInSamePlace)
            {
                interactableGameAction.CreateCardEffect(cardCreature, new Stat(0, false), SelecteCreature,
                    PlayActionType.Choose, investigator, new Localization("CardEffect_Card01528"));

                async Task SelecteCreature()
                {
                    await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
                    await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(this, this, amountDamage: 1).Execute();
                    await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(cardCreature, this, amountDamage: 1).Execute();
                }
            }

            await interactableGameAction.Execute();
        }

        /*******************************************************************/
        private async Task GainStrenghtDeactivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardInvestigator.Strength, 1).Execute();
        }

        private async Task GainStrenghtActivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(cardInvestigator.Strength, 1).Execute();
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
    }
}
