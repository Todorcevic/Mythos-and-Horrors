using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01518 : CardSupply, IDamageable, IFearable
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

            CreateBuff(CardsToBuff, GainStrenghtActivationLogic, GainStrenghtDeactivationLogic, "Buff_Card01518");
            CreateFastActivation(Logic, Condition, PlayActionType.Activate, "Activation_Card01518");
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay.IsTrue) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01518");
            foreach (CardCreature cardCreature in investigator.CreaturesInSamePlace)
            {
                interactableGameAction.CreateCardEffect(cardCreature, new Stat(0, false), SelecteCreature, PlayActionType.Choose, investigator, "CardEffect_Card01518");

                async Task SelecteCreature()
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
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
    }
}
